using Klimaci.DTO.SettingDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _svc;
        public SettingController(ISettingService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 30, [FromQuery] string? group = null, [FromQuery] string? search = null)
            => View(await _svc.PagedAsync(page, pageSize, group, search));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SettingUpsertDTO dto)
        { await _svc.UpsertAsync(dto); TempData["ok"] = "Kaydedildi."; return RedirectToAction(nameof(Index), new { group = dto.Group }); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetByIdAsync(id); if (e is null) return NotFound();
            return View(new SettingUpdateDTO(e.Id, e.Key, e.Value, e.Group, e.Description));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingUpdateDTO dto)
        { await _svc.UpdateAsync(dto); TempData["ok"] = "Güncellendi."; return RedirectToAction(nameof(Index), new { group = dto.Group }); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        { await _svc.DeleteAsync(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }
    }
}
