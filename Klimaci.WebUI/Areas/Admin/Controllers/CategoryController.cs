using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _svc;
        public CategoryController(ICategoryService svc) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> Index([FromQuery] ListQueryDTO q) => View(await _svc.PagedAsync(q ?? new()));
        [HttpGet]
        public IActionResult Create()
            => View(new CategoryAddDTO(Title: "", Slug: null, DisplayOrder: 0));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryAddDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Add(dto); TempData["ok"] = "Kategori eklendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e is null) return NotFound();
            var dto = new CategoryUpdateDTO(
                Id: e.Id,
                Title: e.Title,
                Slug: e.Slug,
                DisplayOrder: e.DisplayOrder
            );
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Update(dto); TempData["ok"] = "Kategori güncellendi."; return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] IdDTO dto)
        { await _svc.Delete(dto.Id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO dto)
        { await _svc.ReorderAsync(dto.Items ?? new()); return Ok(); }

        [HttpPost]
        public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto)
        { return Ok(await _svc.SlugCheckAsync(dto)); }
    }
}
