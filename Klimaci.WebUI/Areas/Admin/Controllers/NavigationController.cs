using Klimaci.DTO.NavigationDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class NavigationController : Controller
    {
        private readonly INavigationService _svc;
        public NavigationController(INavigationService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
            => View(await _svc.PagedMenusAsync(page, pageSize, search));

        [HttpGet] public IActionResult Create() => View(new NavigationMenuAddDTO(Title: "Ana Menü", Slug: "main", IsPublished: true));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NavigationMenuAddDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.AddMenuAsync(dto); TempData["ok"] = "Menü eklendi."; return RedirectToAction(nameof(Index)); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var m = await _svc.GetMenuAsync(id); if (m is null) return NotFound();
            return View(new NavigationMenuUpdateDTO(m.Id, m.Title, m.Slug, m.IsPublished));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NavigationMenuUpdateDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.UpdateMenuAsync(dto); TempData["ok"] = "Kaydedildi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        { await _svc.DeleteMenuAsync(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }

        // Items
        [HttpGet]
        public async Task<IActionResult> Items(Guid id)
        {
            var menu = await _svc.GetMenuAsync(id); if (menu is null) return NotFound();
            ViewBag.Menu = menu;
            return View(await _svc.ListItemsAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> CreateItem(Guid menuId)
        {
            ViewBag.MenuId = menuId;
            return View(new MenuItemAddDTO(NavigationMenuId: menuId, Title: "", Url: "#", Target: null, ParentId: null, DisplayOrder: 0, IsPublished: true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(MenuItemAddDTO dto)
        { if (!ModelState.IsValid) { ViewBag.MenuId = dto.NavigationMenuId; return View(dto); } await _svc.AddItemAsync(dto); TempData["ok"] = "Öğe eklendi."; return RedirectToAction(nameof(Items), new { id = dto.NavigationMenuId }); }

        [HttpGet]
        public async Task<IActionResult> EditItem(Guid id)
        {
            var it = await _svc.GetItemAsync(id); if (it is null) return NotFound();
            return View(new MenuItemUpdateDTO(it.Id, it.NavigationMenuId, it.Title, it.Url, it.Target, it.ParentId, it.DisplayOrder, it.IsPublished));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(MenuItemUpdateDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.UpdateItemAsync(dto); TempData["ok"] = "Kaydedildi."; return RedirectToAction(nameof(Items), new { id = dto.NavigationMenuId }); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem([FromForm] Guid id, [FromForm] Guid menuId)
        { await _svc.DeleteItemAsync(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Items), new { id = menuId }); }

        [HttpPost]
        public async Task<IActionResult> Reorder([FromQuery] Guid menuId, [FromBody] ReorderRequestDTO body)
        { await _svc.ReorderAsync(menuId, body.Items); return Ok(); }

        [HttpPost]
        public async Task<IActionResult> Move([FromBody] MenuItemMoveDTO dto)
        { await _svc.MoveAsync(dto); return Ok(); }
    }
}
