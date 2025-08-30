using Klimaci.Core.Paging;
using Klimaci.DTO.NavigationDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface INavigationService
    {
        // Menu
        Task<PagedResult<NavigationMenu>> PagedMenusAsync(int page = 1, int pageSize = 20, string? search = null);
        Task<NavigationMenu?> GetMenuAsync(Guid id);
        Task<NavigationMenu?> GetMenuBySlugAsync(string slug);
        Task AddMenuAsync(NavigationMenuAddDTO dto);
        Task UpdateMenuAsync(NavigationMenuUpdateDTO dto);
        Task DeleteMenuAsync(Guid id);

        // Items
        Task<List<MenuItem>> ListItemsAsync(Guid menuId);
        Task<MenuItem?> GetItemAsync(Guid id);
        Task AddItemAsync(MenuItemAddDTO dto);
        Task UpdateItemAsync(MenuItemUpdateDTO dto);
        Task DeleteItemAsync(Guid id);
        Task ReorderAsync(Guid menuId, IEnumerable<ReorderItemDTO> items);
        Task MoveAsync(MenuItemMoveDTO dto);
    }
}
