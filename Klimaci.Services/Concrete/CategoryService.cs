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
using Mapster;

namespace Klimaci.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        public CategoryService(IUnitOfWork uow) { _uow = uow; }

        public async Task Add(CategoryAddDTO dto)
        {
            var repo = _uow.Repository<Category>();
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var final = await EnsureUniqueSlugAsync(baseSlug, null);
            var e = dto.Adapt<Category>(); e.Id = Guid.NewGuid(); e.Slug = final;
            await repo.AddAsync(e);
            await _uow.SaveChangesAsync();
        }

        public async Task Update(CategoryUpdateDTO dto)
        {
            var repo = _uow.Repository<Category>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e);
            var baseSlug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            e.Slug = await EnsureUniqueSlugAsync(baseSlug, e.Id);
            await repo.Update(e);
            await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var repo = _uow.Repository<Category>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e);
            await _uow.SaveChangesAsync();
        }

        public Task<List<Category>> GetAll() => _uow.Repository<Category>().GetAll();
        public Task<Category> GetById(Guid id) => _uow.Repository<Category>().GetById(id);

        public async Task<PagedResult<Category>> PagedAsync(ListQueryDTO q)
        {
            q ??= new();
            Expression<Func<Category, bool>> pred = x => string.IsNullOrEmpty(q.Search) || x.Title.Contains(q.Search);
            Func<IQueryable<Category>, IOrderedQueryable<Category>> order = iq =>
                (q.SortBy?.ToLowerInvariant()) switch {
                    "createddate" => q.SortDesc ? iq.OrderByDescending(x => x.CreatedDate) : iq.OrderBy(x => x.CreatedDate),
                    "displayorder" => q.SortDesc ? iq.OrderByDescending(x => x.DisplayOrder) : iq.OrderBy(x => x.DisplayOrder),
                    _ => q.SortDesc ? iq.OrderByDescending(x => x.Title) : iq.OrderBy(x => x.Title)
                };
            var (items, total) = await _uow.Repository<Category>().PagedAsync(q.Page, q.PageSize, pred, order);
            return new(items, total, q.Page, q.PageSize);
        }

        public async Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto)
        {
            var s = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Make(dto.Title) : SlugHelper.Make(dto.Slug);
            var f = await EnsureUniqueSlugAsync(s, dto.ExcludeId);
            return new SlugCheckResultDTO(string.Equals(s, f, StringComparison.Ordinal), f);
        }

        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items)
        {
            var repo = _uow.Repository<Category>();
            foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>())
            {
                var e = await repo.GetById(it.Id); if (e is null) continue;
                e.DisplayOrder = it.DisplayOrder; await repo.Update(e);
            }
            await _uow.SaveChangesAsync();
        }

        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId)
        {
            var repo = _uow.Repository<Category>(); var slug = baseSlug; var i = 1;
            while (await repo.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value)))
            {
                var suf = "-" + i.ToString("00"); var cut = Math.Max(0, 220 - suf.Length);
                slug = (baseSlug.Length > cut ? baseSlug[..cut] : baseSlug) + suf; i++;
            }
            return slug;
        }
    }
}
