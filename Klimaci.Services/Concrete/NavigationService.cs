using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO.NavigationDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class NavigationService : INavigationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;
        public NavigationService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        // MENUS
        public async Task<PagedResult<NavigationMenu>> PagedMenusAsync(int page = 1, int pageSize = 20, string? search = null)
        {
            Expression<Func<NavigationMenu, bool>> pred = x => string.IsNullOrWhiteSpace(search) || x.Title.Contains(search) || x.Slug.Contains(search);
            var (items, total) = await _uow.Repository<NavigationMenu>().PagedAsync(page, pageSize, pred, iq => iq.OrderBy(x => x.Title));
            return new(items, total, page, pageSize);
        }

        public Task<NavigationMenu?> GetMenuAsync(Guid id) => _uow.Repository<NavigationMenu>().GetById(id);
        public async Task<NavigationMenu?> GetMenuBySlugAsync(string slug) =>
            (await _uow.Repository<NavigationMenu>().GetBy(x => x.Slug == slug)).FirstOrDefault();

        public async Task AddMenuAsync(NavigationMenuAddDTO dto)
        {
            var repo = _uow.Repository<NavigationMenu>();
            var e = dto.Adapt<NavigationMenu>(); e.Id = Guid.NewGuid();
            await repo.AddAsync(e); await _uow.SaveChangesAsync();
        }

        public async Task UpdateMenuAsync(NavigationMenuUpdateDTO dto)
        {
            var repo = _uow.Repository<NavigationMenu>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e); await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(Guid id)
        {
            // items cascade (Mapping’de Cascade var)
            var repo = _uow.Repository<NavigationMenu>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }

        // ITEMS
        public async Task<List<MenuItem>> ListItemsAsync(Guid menuId)
        {
            var q = await _uow.Repository<MenuItem>().GetBy(x => x.NavigationMenuId == menuId);
            return await q.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public Task<MenuItem?> GetItemAsync(Guid id) => _uow.Repository<MenuItem>().GetById(id);

        public async Task AddItemAsync(MenuItemAddDTO dto)
        {
            var repo = _uow.Repository<MenuItem>();
            var e = dto.Adapt<MenuItem>(); e.Id = Guid.NewGuid();
            await repo.AddAsync(e); await _uow.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(MenuItemUpdateDTO dto)
        {
            var repo = _uow.Repository<MenuItem>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e); await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            // child var mı? Restrict delete; önce altları sil
            await _ctx.Set<MenuItem>().Where(x => x.ParentId == id).ExecuteDeleteAsync();
            var repo = _uow.Repository<MenuItem>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }

        public async Task ReorderAsync(Guid menuId, IEnumerable<ReorderItemDTO> items)
        {
            var repo = _uow.Repository<MenuItem>();
            foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>())
            {
                var e = await repo.GetById(it.Id); if (e is null || e.NavigationMenuId != menuId) continue;
                e.DisplayOrder = it.DisplayOrder; await repo.Update(e);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task MoveAsync(MenuItemMoveDTO dto)
        {
            var repo = _uow.Repository<MenuItem>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            e.ParentId = dto.NewParentId;
            await repo.Update(e); await _uow.SaveChangesAsync();
        }
    }
}
