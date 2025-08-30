using Klimaci.Core.Abstracts;
using Klimaci.Core.Extensions;
using Klimaci.Core.Paging;
using Klimaci.DTO.PartnerDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Klimaci.Services.Concrete
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _uow; private readonly IEFContext _ctx;
        public PartnerService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        public async Task Add(PartnerAddDTO dto)
        {
            var repo = _uow.Repository<Partner>();
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var final = await EnsureUniqueSlugAsync(baseSlug, null);
            var e = dto.Adapt<Partner>(); e.Id = Guid.NewGuid(); e.Slug = final;
            await repo.AddAsync(e);
            if (dto.MediaIds is { Count: > 0 }) await UpsertGallery(e.Id, dto.MediaIds.Distinct().ToList(), dto.CoverMediaId);
            await _uow.SaveChangesAsync();
        }
        public async Task Update(PartnerUpdateDTO dto)
        {
            var r = _uow.Repository<Partner>(); var e = await r.GetById(dto.Id); if (e == null) return;
            dto.Adapt(e);
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            e.Slug = await EnsureUniqueSlugAsync(baseSlug, e.Id);
            await r.Update(e);
            if (dto.MediaIds is not null || dto.CoverMediaId.HasValue)
                await UpsertGallery(e.Id, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId);
            await _uow.SaveChangesAsync();
        }
        public async Task Delete(Guid id) { var r = _uow.Repository<Partner>(); var e = await r.GetById(id); if (e == null) return; await r.Delete(e); await _ctx.Set<PartnerMedia>().Where(x => x.PartnerId == id).ExecuteDeleteAsync(); await _uow.SaveChangesAsync(); }
        public Task<List<Partner>> GetAll() => _uow.Repository<Partner>().GetAll();
        public Task<Partner> GetById(Guid id) => _uow.Repository<Partner>().GetById(id);

        public async Task<PagedResult<Partner>> PagedAsync(ListQueryDTO q)
        {
            q ??= new();
            Expression<Func<Partner, bool>> pred = x => (string.IsNullOrEmpty(q.Search) || x.Title.Contains(q.Search)) &&
                                                       (!q.OnlyPublished.HasValue || x.IsPublished == q.OnlyPublished.Value);
            Func<IQueryable<Partner>, IOrderedQueryable<Partner>> order = iq =>
                (q.SortBy?.ToLowerInvariant()) switch {
                    "createddate" => q.SortDesc ? iq.OrderByDescending(x => x.CreatedDate) : iq.OrderBy(x => x.CreatedDate),
                    "displayorder" => q.SortDesc ? iq.OrderByDescending(x => x.DisplayOrder) : iq.OrderBy(x => x.DisplayOrder),
                    _ => q.SortDesc ? iq.OrderByDescending(x => x.Title) : iq.OrderBy(x => x.Title)
                };
            (IReadOnlyList<Partner> items, int total) = await _uow.Repository<Partner>().PagedAsync(q.Page, q.PageSize, pred, order);
            return new(items, total, q.Page, q.PageSize);
        }

        public async Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto)
        {
            var s = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var f = await EnsureUniqueSlugAsync(s, dto.ExcludeId);
            return new SlugCheckResultDTO(string.Equals(s, f, StringComparison.Ordinal), f);
        }

        public async Task<List<GalleryItemDTO>> GetGalleryAsync(Guid id)
        {
            return await _ctx.Set<PartnerMedia>().Where(x => x.PartnerId == id).Include(x => x.Media).OrderBy(x => x.DisplayOrder)
                .Select(x => new GalleryItemDTO(x.MediaId, x.Media.FileUrl, x.IsCover, x.DisplayOrder, x.Media.AltText, x.Media.Title)).ToListAsync();
        }

        public async Task GalleryUpdateAsync(GalleryUpdateDTO dto) { await UpsertGallery(dto.OwnerId, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId); await _uow.SaveChangesAsync(); }
        public async Task TogglePublishAsync(Guid id, bool v) { var r = _uow.Repository<Partner>(); var e = await r.GetById(id); if (e == null) return; e.IsPublished = v; await r.Update(e); await _uow.SaveChangesAsync(); }
        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items) { var r = _uow.Repository<Partner>(); foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>()) { var e = await r.GetById(it.Id); if (e == null) continue; e.DisplayOrder = it.DisplayOrder; await r.Update(e); } await _uow.SaveChangesAsync(); }

        // helpers
        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId)
        {
            var repo = _uow.Repository<Partner>(); var slug = baseSlug; var i = 1;
            while (await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value)))
            {
                var suf = "-" + i.ToString("00"); var cut = Math.Max(0, 220 - suf.Length); slug = (baseSlug.Length > cut ? baseSlug[..cut] : baseSlug) + suf; i++;
            }
            return slug;
        }
        private async Task UpsertGallery(Guid id, List<Guid> mediaIds, Guid? coverId)
        {
            var set = _ctx.Set<PartnerMedia>(); await set.Where(x => x.PartnerId == id).ExecuteDeleteAsync();
            if (mediaIds.Count > 0)
            {
                var list = mediaIds.Select((mid, idx) => new PartnerMedia { PartnerId = id, MediaId = mid, DisplayOrder = idx, IsCover = coverId.HasValue ? mid == coverId.Value : idx == 0 }).ToList();
                await set.AddRangeAsync(list);
            }
        }
    }
}
