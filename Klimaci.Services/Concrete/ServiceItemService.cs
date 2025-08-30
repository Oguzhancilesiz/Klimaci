using Klimaci.Core.Abstracts;
using Klimaci.Core.Extensions;
using Klimaci.Core.Paging;
using Klimaci.DTO.ServiceItemDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Klimaci.Services.Concrete
{
    public class ServiceItemService : IServiceItemService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;
        public ServiceItemService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        public async Task Add(ServiceItemAddDTO dto)
        {
            var repo = _uow.Repository<ServiceItem>();
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var finalSlug = await EnsureUniqueSlugAsync(baseSlug, null);

            var e = dto.Adapt<ServiceItem>();
            e.Id = Guid.NewGuid();
            e.Slug = finalSlug;

            await repo.AddAsync(e);
            if (dto.MediaIds is { Count: > 0 }) await UpsertGallery(e.Id, dto.MediaIds.Distinct().ToList(), dto.CoverMediaId);
            await _uow.SaveChangesAsync();
        }

        public async Task Update(ServiceItemUpdateDTO dto)
        {
            var repo = _uow.Repository<ServiceItem>();
            var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e);
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            e.Slug = await EnsureUniqueSlugAsync(baseSlug, e.Id);
            await repo.Update(e);

            if (dto.MediaIds is not null || dto.CoverMediaId.HasValue)
                await UpsertGallery(e.Id, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId);

            await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var repo = _uow.Repository<ServiceItem>();
            var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e);
            await _ctx.Set<ServiceMedia>().Where(x => x.ServiceItemId == id).ExecuteDeleteAsync();
            await _uow.SaveChangesAsync();
        }

        public Task<List<ServiceItem>> GetAll() => _uow.Repository<ServiceItem>().GetAll();
        public Task<ServiceItem> GetById(Guid id) => _uow.Repository<ServiceItem>().GetById(id);

        public async Task<PagedResult<ServiceItem>> PagedAsync(ListQueryDTO q)
        {
            q ??= new();
            Expression<Func<ServiceItem, bool>> pred = x =>
                (string.IsNullOrEmpty(q.Search) || x.Title.Contains(q.Search)) &&
                (!q.OnlyPublished.HasValue || x.IsPublished == q.OnlyPublished.Value);

            Func<IQueryable<ServiceItem>, IOrderedQueryable<ServiceItem>> order = iq =>
                (q.SortBy?.ToLowerInvariant()) switch {
                    "createddate" => q.SortDesc ? iq.OrderByDescending(x => x.CreatedDate) : iq.OrderBy(x => x.CreatedDate),
                    "displayorder" => q.SortDesc ? iq.OrderByDescending(x => x.DisplayOrder) : iq.OrderBy(x => x.DisplayOrder),
                    _ => q.SortDesc ? iq.OrderByDescending(x => x.Title) : iq.OrderBy(x => x.Title)
                };

            (IReadOnlyList<ServiceItem> items, int total) = await _uow.Repository<ServiceItem>().PagedAsync(q.Page, q.PageSize, pred, order);
            return new(items, total, q.Page, q.PageSize);
        }

        public async Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto)
        {
            var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var final = await EnsureUniqueSlugAsync(slug, dto.ExcludeId);
            return new SlugCheckResultDTO(string.Equals(slug, final, StringComparison.Ordinal), final);
        }

        public async Task<List<GalleryItemDTO>> GetGalleryAsync(Guid id)
        {
            return await _ctx.Set<ServiceMedia>()
                .Where(x => x.ServiceItemId == id)
                .Include(x => x.Media)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new GalleryItemDTO(x.MediaId, x.Media.FileUrl, x.IsCover, x.DisplayOrder, x.Media.AltText, x.Media.Title))
                .ToListAsync();
        }

        public async Task GalleryUpdateAsync(GalleryUpdateDTO dto)
        {
            await UpsertGallery(dto.OwnerId, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId);
            await _uow.SaveChangesAsync();
        }

        public async Task TogglePublishAsync(Guid id, bool v) { var r = _uow.Repository<ServiceItem>(); var e = await r.GetById(id); if (e == null) return; e.IsPublished = v; await r.Update(e); await _uow.SaveChangesAsync(); }
        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items) { var r = _uow.Repository<ServiceItem>(); foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>()) { var e = await r.GetById(it.Id); if (e == null) continue; e.DisplayOrder = it.DisplayOrder; await r.Update(e); } await _uow.SaveChangesAsync(); }

        // helpers
        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId)
        {
            var repo = _uow.Repository<ServiceItem>(); var slug = baseSlug; var i = 1;
            while (await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value)))
            {
                var suf = "-" + i.ToString("00"); var cut = Math.Max(0, 220 - suf.Length); slug = (baseSlug.Length > cut ? baseSlug[..cut] : baseSlug) + suf; i++;
            }
            return slug;
        }
        private async Task UpsertGallery(Guid ownerId, List<Guid> mediaIds, Guid? coverId)
        {
            var set = _ctx.Set<ServiceMedia>();
            await set.Where(x => x.ServiceItemId == ownerId).ExecuteDeleteAsync();
            if (mediaIds.Count > 0)
            {
                var list = mediaIds.Select((mid, idx) => new ServiceMedia { ServiceItemId = ownerId, MediaId = mid, DisplayOrder = idx, IsCover = coverId.HasValue ? mid == coverId.Value : idx == 0 }).ToList();
                await set.AddRangeAsync(list);
            }
        }
    }
}
