using Klimaci.DTO.RoleDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _svc;
        public RoleController(IRoleService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
            => View(await _svc.PagedAsync(page, pageSize, search));

        [HttpGet] public IActionResult Create() => View(new RoleAddDTO());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleAddDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var (ok, err) = await _svc.AddAsync(dto);
            if (!ok) { TempData["err"] = err; return View(dto); }
            TempData["ok"] = "Rol eklendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var r = await _svc.GetByIdAsync(id); if (r is null) return NotFound();
            return View(new RoleUpdateDTO { Id = r.Id, Name = r.Name! });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var (ok, err) = await _svc.UpdateAsync(dto);
            if (!ok) { TempData["err"] = err; return View(dto); }
            TempData["ok"] = "Rol güncellendi."; return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            var (ok, err) = await _svc.DeleteAsync(id);
            TempData[ok ? "ok" : "err"] = ok ? "Rol silindi." : err;
            return RedirectToAction(nameof(Index));
        }
    }
}
