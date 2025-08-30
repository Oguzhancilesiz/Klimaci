using Klimaci.Core.Abstracts;
using Klimaci.Core.Extensions;
using Klimaci.Core.Paging;
using Klimaci.DTO.ProjectDTOs;
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
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;

        public ProjectService(IUnitOfWork uow, IEFContext ctx)
        {
            _uow = uow;
            _ctx = ctx;
        }

        // ========== CRUD ==========
        public async Task Add(ProjectAddDTO dto)
        {
            var repo = _uow.Repository<Project>();

            // Slug üret + benzersizleştir
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var finalSlug = await EnsureUniqueSlugAsync(baseSlug, null);

            var entity = dto.Adapt<Project>();
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            entity.Slug = finalSlug;

            await repo.AddAsync(entity);

            if (dto.MediaIds is { Count: > 0 })
                await UpsertGallery(entity.Id, dto.MediaIds, dto.CoverMediaId);

            await _uow.SaveChangesAsync();
        }

        public async Task Update(ProjectUpdateDTO dto)
        {
            var repo = _uow.Repository<Project>();
            var entity = await repo.GetById(dto.Id);
            if (entity == null) return;

            // Map alanlar
            dto.Adapt(entity);

            // Slug null/boşsa üret, varsa normalize et; benzersizleştir
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            entity.Slug = await EnsureUniqueSlugAsync(baseSlug, entity.Id);

            await repo.Update(entity);

            // Galeri güncelle
            if (dto.MediaIds is not null || dto.CoverMediaId.HasValue)
            {
                var ids = (dto.MediaIds ?? new List<Guid>()).Distinct().ToList();
                await UpsertGallery(entity.Id, ids, dto.CoverMediaId);
            }

            await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var repo = _uow.Repository<Project>();
            var entity = await repo.GetById(id);
            if (entity == null) return;

            // Soft delete
            await repo.Delete(entity);

            // Join'leri fiziksel sil (soft delete yok)
            await _ctx.Set<ProjectMedia>()
                      .Where(x => x.ProjectId == id)
                      .ExecuteDeleteAsync();

            await _uow.SaveChangesAsync();
        }

        public Task<List<Project>> GetAll() => _uow.Repository<Project>().GetAll();

        public Task<Project> GetById(Guid id) => _uow.Repository<Project>().GetById(id);

        // ========== Admin yardımcıları ==========
        public async Task<PagedResult<Project>> PagedAsync(ListQueryDTO q)
        {
            q ??= new();
            var repo = _uow.Repository<Project>();

            Expression<Func<Project, bool>> pred = x =>
                (string.IsNullOrEmpty(q.Search) || x.Title.Contains(q.Search)) &&
                (!q.OnlyPublished.HasValue || x.IsPublished == q.OnlyPublished.Value) &&
                (!q.BrandId.HasValue || x.BrandId == q.BrandId);

            Func<IQueryable<Project>, IOrderedQueryable<Project>> order = iq =>
                (q.SortBy?.ToLowerInvariant()) switch
                {
                    "createddate" => q.SortDesc ? iq.OrderByDescending(x => x.CreatedDate) : iq.OrderBy(x => x.CreatedDate),
                    "displayorder" => q.SortDesc ? iq.OrderByDescending(x => x.DisplayOrder) : iq.OrderBy(x => x.DisplayOrder),
                    "city" => q.SortDesc ? iq.OrderByDescending(x => x.City) : iq.OrderBy(x => x.City),
                    _ => q.SortDesc ? iq.OrderByDescending(x => x.Title) : iq.OrderBy(x => x.Title)
                };

            var (items, total) = await repo.PagedAsync(q.Page, q.PageSize, pred, order);
            return new PagedResult<Project>(items, total, q.Page, q.PageSize);
        }

        public async Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto)
        {
            var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var final = await EnsureUniqueSlugAsync(slug, dto.ExcludeId);
            var isUnique = string.Equals(slug, final, StringComparison.Ordinal);
            return new SlugCheckResultDTO(isUnique, final);
        }

        public async Task TogglePublishAsync(Guid id, bool isPublished)
        {
            var repo = _uow.Repository<Project>();
            var e = await repo.GetById(id);
            if (e == null) return;
            e.IsPublished = isPublished;
            await repo.Update(e);
            await _uow.SaveChangesAsync();
        }

        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items)
        {
            var repo = _uow.Repository<Project>();
            foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>())
            {
                var e = await repo.GetById(it.Id);
                if (e == null) continue;
                e.DisplayOrder = it.DisplayOrder;
                await repo.Update(e);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task GalleryUpdateAsync(GalleryUpdateDTO dto)
        {
            var ids = (dto.MediaIds ?? new List<Guid>()).Distinct().ToList();
            await UpsertGallery(dto.OwnerId, ids, dto.CoverMediaId);
            await _uow.SaveChangesAsync();
        }

        // ========== Helpers ==========
        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId)
        {
            var repo = _uow.Repository<Project>();
            var slug = baseSlug;
            var i = 1;

            bool exists = await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value));
            while (exists)
            {
                var suffix = "-" + i.ToString("00");
                var cut = Math.Max(0, 220 - suffix.Length); // Slug max 220
                slug = (baseSlug.Length > cut ? baseSlug[..cut] : baseSlug) + suffix;
                exists = await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value));
                i++;
            }
            return slug;
        }

        private async Task UpsertGallery(Guid projectId, List<Guid> mediaIds, Guid? coverId)
        {
            var set = _ctx.Set<ProjectMedia>();

            // Sil ve yeniden yaz (basit ve deterministik)
            await set.Where(x => x.ProjectId == projectId).ExecuteDeleteAsync();

            if (mediaIds is { Count: > 0 })
            {
                var list = mediaIds
                    .Distinct()
                    .Select((mid, idx) => new ProjectMedia
                    {
                        ProjectId = projectId,
                        MediaId = mid,
                        DisplayOrder = idx,
                        IsCover = coverId.HasValue ? mid == coverId.Value : idx == 0
                    }).ToList();

                await set.AddRangeAsync(list);
            }
        }

        public async Task<List<GalleryItemDTO>> GetGalleryAsync(Guid projectId)
        {
            var list = await _ctx.Set<ProjectMedia>()
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.Media)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new GalleryItemDTO(
                    x.MediaId,
                    x.Media.FileUrl,
                    x.IsCover,
                    x.DisplayOrder,
                    x.Media.AltText,
                    x.Media.Title
                ))
                .ToListAsync();

            return list;
        }
    }
}
