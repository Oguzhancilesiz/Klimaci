using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles="Admin")]
    public class AppointmentRequestController : Controller
    {
        private readonly IAppointmentRequestService _svc;
        public AppointmentRequestController(IAppointmentRequestService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 30, string? search = null, bool onlyUnprocessed = false)
            => View(await _svc.PagedAsync(page, pageSize, search, onlyUnprocessed));

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var e = await _svc.GetByIdAsync(id); if (e is null) return NotFound();
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(Guid id, bool isProcessed, string? notes)
        {
            await _svc.ProcessAsync(id, isProcessed, notes);
            TempData["ok"] = "Randevu talebi güncellendi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _svc.DeleteAsync(id);
            TempData["ok"] = "Talep silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
