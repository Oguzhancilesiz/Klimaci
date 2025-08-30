using Klimaci.DTO.TestimonialDTOs;
using Klimaci.DTO;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _svc;
        public TestimonialController(ITestimonialService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] bool desc = false)
            => View(await _svc.PagedAsync(new ListQueryDTO(page, pageSize, search, sortBy, desc)));

        [HttpGet]
        public IActionResult Create() => View(new TestimonialAddDTO(FullName: "",
            Title: "", Company: null, Content: "", Rating: null,
            Slug: null, SeoTitle: null, SeoDescription: null,
            DisplayOrder: 0, IsPublished: true, MediaId: null));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialAddDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Add(dto); TempData["ok"] = "Referans eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e is null) return NotFound();
            var dto = new TestimonialUpdateDTO(
                Id: e.Id,FullName : e.FullName, Title: e.Title, Company: e.Company, Content: e.Content, Rating: e.Rating,
                Slug: e.Slug, SeoTitle: e.SeoTitle, SeoDescription: e.SeoDescription,
                DisplayOrder: e.DisplayOrder, IsPublished: e.IsPublished, MediaId: e.MediaId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestimonialUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.Update(dto); TempData["ok"] = "Kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            await _svc.Delete(id); TempData["ok"] = "Silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto)
            => Json(await _svc.SlugCheckAsync(dto));

        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO body)
        {
            await _svc.ReorderAsync(body.Items); return Ok();
        }
    }
}
