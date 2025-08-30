using Klimaci.DTO;
using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO.FaqDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class FaqController : Controller
    {
        private readonly IFaqService _svc;
        public FaqController(IFaqService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 30, [FromQuery] string? search = null)
        => View(await _svc.PagedAsync(page, pageSize, search));

        [HttpGet] public IActionResult Create() => View(new FaqItemAddDTO(Question: "", Answer: "", DisplayOrder: 0, IsPublished: true));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FaqItemAddDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.Add(dto); TempData["ok"] = "Eklendi."; return RedirectToAction(nameof(Index)); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e is null) return NotFound();
            return View(new FaqItemUpdateDTO(e.Id, e.Question, e.Answer, e.DisplayOrder, e.IsPublished));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FaqItemUpdateDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.Update(dto); TempData["ok"] = "Güncellendi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        { await _svc.Delete(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO body)
        { await _svc.ReorderAsync(body.Items); return Ok(); }
    }
}
