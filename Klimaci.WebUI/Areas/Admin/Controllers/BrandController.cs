using Klimaci.DTO.ProjectDTOs;
using Klimaci.DTO;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _svc;
        public BrandController(IBrandService svc) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> Index([FromQuery] ListQueryDTO q) => View(await _svc.PagedAsync(q ?? new()));

        [HttpGet]
        public IActionResult Create()
     => View(new BrandAddDTO(
         Title: "",
         Website: null,
         Slug: null,
         SeoTitle: null,
         SeoDescription: null,
         DisplayOrder: 0,
         IsPublished: true
     ));


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandAddDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Add(dto); TempData["ok"] = "Marka eklendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id);
            if (e is null) return NotFound();

            var dto = new BrandUpdateDTO(
                Id: e.Id,
                Title: e.Title,
                Website: e.Website,          // entity’de ne varsa
                Slug: e.Slug,
                SeoTitle: e.SeoTitle,
                SeoDescription: e.SeoDescription,
                DisplayOrder: e.DisplayOrder,
                IsPublished: e.IsPublished
            );
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Update(dto); TempData["ok"] = "Marka güncellendi."; return RedirectToAction(nameof(Index));
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
