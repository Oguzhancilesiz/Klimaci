using Klimaci.Core.Abstracts;
using Klimaci.Core.Extensions;
using Klimaci.Core.Paging;
using Klimaci.DTO.BlogDTOs;
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
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _uow; private readonly IEFContext _ctx;
        public BlogService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        public async Task Add(BlogPostAddDTO dto)
        {
            var repo = _uow.Repository<BlogPost>();
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var final = await EnsureUniqueSlugAsync(baseSlug, null);
            var e = dto.Adapt<BlogPost>(); e.Id = Guid.NewGuid(); e.Slug = final;
            await repo.AddAsync(e);

            await UpsertCategories(e.Id, dto.CategoryIds ?? new());
            await UpsertTags(e.Id, dto.TagIds ?? new());
            if (dto.MediaIds is { Count: > 0 }) await UpsertGallery(e.Id, dto.MediaIds.Distinct().ToList(), dto.CoverMediaId);

            await _uow.SaveChangesAsync();
        }

        public async Task Update(BlogPostUpdateDTO dto)
        {
            var repo = _uow.Repository<BlogPost>(); var e = await repo.GetById(dto.Id); if (e == null) return;
            dto.Adapt(e);
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            e.Slug = await EnsureUniqueSlugAsync(baseSlug, e.Id);
            await repo.Update(e);

            if (dto.CategoryIds is not null) await UpsertCategories(e.Id, dto.CategoryIds);
            if (dto.TagIds is not null) await UpsertTags(e.Id, dto.TagIds);
            if (dto.MediaIds is not null || dto.CoverMediaId.HasValue)
                await UpsertGallery(e.Id, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId);

            await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var r = _uow.Repository<BlogPost>(); var e = await r.GetById(id); if (e == null) return;
            await r.Delete(e);
            await _ctx.Set<BlogPostCategory>().Where(x => x.BlogPostId == id).ExecuteDeleteAsync();
            await _ctx.Set<BlogPostTag>().Where(x => x.BlogPostId == id).ExecuteDeleteAsync();
            await _ctx.Set<BlogPostMedia>().Where(x => x.BlogPostId == id).ExecuteDeleteAsync();
            await _uow.SaveChangesAsync();
        }

        public Task<List<BlogPost>> GetAll() => _uow.Repository<BlogPost>().GetAll();
        public Task<BlogPost> GetById(Guid id) => _uow.Repository<BlogPost>().GetById(id);

        public async Task<PagedResult<BlogPost>> PagedAsync(BlogListQueryDTO q)
        {
            q ??= new();
            Expression<Func<BlogPost, bool>> pred = x =>
                (string.IsNullOrEmpty(q.Search) || x.Title.Contains(q.Search)) &&
                (!q.OnlyPublished.HasValue || x.IsPublished == q.OnlyPublished.Value) &&
                (!q.CategoryId.HasValue || _ctx.Set<BlogPostCategory>().Any(j => j.BlogPostId == x.Id && j.CategoryId == q.CategoryId)) &&
                (!q.TagId.HasValue || _ctx.Set<BlogPostTag>().Any(j => j.BlogPostId == x.Id && j.TagId == q.TagId)) &&
                (!q.From.HasValue || (x.PublishDate ?? x.CreatedDate) >= q.From) &&
                (!q.To.HasValue || (x.PublishDate ?? x.CreatedDate) <= q.To);

            Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> order = iq =>
                (q.SortBy?.ToLowerInvariant()) switch {
                    "publishdate" => q.SortDesc ? iq.OrderByDescending(x => x.PublishDate) : iq.OrderBy(x => x.PublishDate),
                    "createddate" => q.SortDesc ? iq.OrderByDescending(x => x.CreatedDate) : iq.OrderBy(x => x.CreatedDate),
                    "displayorder" => q.SortDesc ? iq.OrderByDescending(x => x.DisplayOrder) : iq.OrderBy(x => x.DisplayOrder),
                    _ => q.SortDesc ? iq.OrderByDescending(x => x.Title) : iq.OrderBy(x => x.Title)
                };

            (IReadOnlyList<BlogPost> items, int total) = await _uow.Repository<BlogPost>().PagedAsync(q.Page, q.PageSize, pred, order);
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
            return await _ctx.Set<BlogPostMedia>().Where(x => x.BlogPostId == id)
                .Include(x => x.Media).OrderBy(x => x.DisplayOrder)
                .Select(x => new GalleryItemDTO(x.MediaId, x.Media.FileUrl, x.IsCover, x.DisplayOrder, x.Media.AltText, x.Media.Title)).ToListAsync();
        }

        public async Task GalleryUpdateAsync(GalleryUpdateDTO dto) { await UpsertGallery(dto.OwnerId, (dto.MediaIds ?? new()).Distinct().ToList(), dto.CoverMediaId); await _uow.SaveChangesAsync(); }
        public async Task TogglePublishAsync(Guid id, bool v) { var r = _uow.Repository<BlogPost>(); var e = await r.GetById(id); if (e == null) return; e.IsPublished = v; await r.Update(e); await _uow.SaveChangesAsync(); }
        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items) { var r = _uow.Repository<BlogPost>(); foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>()) { var e = await r.GetById(it.Id); if (e == null) continue; e.DisplayOrder = it.DisplayOrder; await r.Update(e); } await _uow.SaveChangesAsync(); }

        // helpers
        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId)
        {
            var repo = _uow.Repository<BlogPost>(); var slug = baseSlug; var i = 1;
            while (await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value)))
            {
                var suf = "-" + i.ToString("00"); var cut = Math.Max(0, 220 - suf.Length); slug = (baseSlug.Length > cut ? baseSlug[..cut] : baseSlug) + suf; i++;
            }
            return slug;
        }
        private async Task UpsertGallery(Guid id, List<Guid> mediaIds, Guid? coverId)
        {
            var set = _ctx.Set<BlogPostMedia>(); await set.Where(x => x.BlogPostId == id).ExecuteDeleteAsync();
            if (mediaIds.Count > 0)
            {
                var list = mediaIds.Select((mid, idx) => new BlogPostMedia { BlogPostId = id, MediaId = mid, DisplayOrder = idx, IsCover = coverId.HasValue ? mid == coverId.Value : idx == 0 }).ToList();
                await set.AddRangeAsync(list);
            }
        }
        private async Task UpsertCategories(Guid postId, IEnumerable<Guid> catIds)
        {
            var set = _ctx.Set<BlogPostCategory>(); await set.Where(x => x.BlogPostId == postId).ExecuteDeleteAsync();
            var ids = catIds.Distinct().ToList(); if (ids.Count == 0) return;
            await set.AddRangeAsync(ids.Select(id => new BlogPostCategory { BlogPostId = postId, CategoryId = id }));
        }
        private async Task UpsertTags(Guid postId, IEnumerable<Guid> tagIds)
        {
            var set = _ctx.Set<BlogPostTag>(); await set.Where(x => x.BlogPostId == postId).ExecuteDeleteAsync();
            var ids = tagIds.Distinct().ToList(); if (ids.Count == 0) return;
            await set.AddRangeAsync(ids.Select(id => new BlogPostTag { BlogPostId = postId, TagId = id }));
        }
    }
}
