using Klimaci.DTO.ContactMessageDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ContactMessageController : Controller
    {
        private readonly IContactMessageService _svc;
        public ContactMessageController(IContactMessageService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] bool onlyUnprocessed = false)
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
            await _svc.ProcessAsync(new ContactProcessDTO(id, isProcessed, notes));
            TempData["ok"] = "Mesaj güncellendi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            await _svc.DeleteAsync(id);
            TempData["ok"] = "Mesaj silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
