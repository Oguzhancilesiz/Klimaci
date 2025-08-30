using Klimaci.DTO.NewsletterDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _svc;
        public NewsletterController(INewsletterService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 40,
            [FromQuery] string? search = null,
            [FromQuery] bool onlyActive = false)
        {
            var model = await _svc.PagedAsync(page, pageSize, search, onlyActive);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetByIdAsync(id);
            if (e is null) return NotFound();
            return View(new NewsletterUpdateDTO(e.Id, e.Email, e.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsletterUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["err"] = "Formu kontrol et.";
                return View(dto);
            }
            await _svc.UpdateAsync(dto);
            TempData["ok"] = "Güncellendi.";
            return RedirectToAction(nameof(Edit), new { id = dto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([FromForm] Guid id)
        {
            var (ok, error) = await _svc.AdminConfirmAsync(id);
            TempData[ok ? "ok" : "err"] = ok ? "Onaylandı." : error;
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe([FromForm] Guid id)
        {
            var (ok, error) = await _svc.UnsubscribeAsync(id);
            TempData[ok ? "ok" : "err"] = ok ? "Çıkarıldı." : error;
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            await _svc.DeleteAsync(id);
            TempData["ok"] = "Silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
