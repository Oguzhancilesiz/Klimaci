using Klimaci.DTO.UrlRedirectDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class UrlRedirectController : Controller
    {
        private readonly IUrlRedirectService _svc;
        public UrlRedirectController(IUrlRedirectService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 30, [FromQuery] string? search = null)
            => View(await _svc.PagedAsync(page, pageSize, search));

        [HttpGet] public IActionResult Create() => View(new UrlRedirectAddDTO(OldPath: "/", NewPath: "/", StatusCode: 301, IsPublished: true));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UrlRedirectAddDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.AddAsync(dto); TempData["ok"] = "Eklendi."; return RedirectToAction(nameof(Index)); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetByIdAsync(id); if (e is null) return NotFound();
            return View(new UrlRedirectUpdateDTO(e.Id, e.OldPath, e.NewPath, e.StatusCode, e.IsPublished));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UrlRedirectUpdateDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.UpdateAsync(dto); TempData["ok"] = "Güncellendi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        { await _svc.DeleteAsync(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }
    }
}
