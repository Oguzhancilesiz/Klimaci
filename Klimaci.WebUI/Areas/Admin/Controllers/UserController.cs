using Klimaci.DTO.UserDTOs;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _users;
        private readonly IRoleService _roles;
        public UserController(IUserService users, IRoleService roles) { _users = users; _roles = roles; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] bool desc = false)
            => View(await _users.PagedAsync(page, pageSize, search, sortBy, desc));

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllRoles = (await _roles.GetAllAsync()).Select(r => r.Name!).ToList();
            return View(new UserAddDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAddDTO dto)
        {
            if (!ModelState.IsValid) { ViewBag.AllRoles = (await _roles.GetAllAsync()).Select(r => r.Name!).ToList(); return View(dto); }
            var (ok, err) = await _users.AddAsync(dto);
            if (!ok) { TempData["err"] = err; ViewBag.AllRoles = (await _roles.GetAllAsync()).Select(r => r.Name!).ToList(); return View(dto); }
            TempData["ok"] = "Kullanıcı eklendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var u = await _users.GetByIdAsync(id); if (u is null) return NotFound();
            var dto = new UserUpdateDTO { Id = u.Id, UserName = u.UserName!, Email = u.Email!, IsActive = u.LockoutEnd is null };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var (ok, err) = await _users.UpdateAsync(dto);
            if (!ok) { TempData["err"] = err; return View(dto); }
            TempData["ok"] = "Kullanıcı güncellendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Roles(Guid id)
        {
            var u = await _users.GetByIdAsync(id); if (u is null) return NotFound();
            var all = (await _roles.GetAllAsync()).Select(x => x.Name!).ToList();
            var has = await _users.GetUserRolesAsync(id);
            ViewBag.AllRoles = all;
            return View(new AssignRolesDTO { UserId = id, RoleNames = has });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(AssignRolesDTO dto)
        {
            var (ok, err) = await _users.UpdateUserRolesAsync(dto);
            if (!ok) { TempData["err"] = err; return RedirectToAction(nameof(Roles), new { id = dto.UserId }); }
            TempData["ok"] = "Roller güncellendi."; return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var (ok, err) = await _users.ResetPasswordAsync(dto);
            TempData[ok ? "ok" : "err"] = ok ? "Şifre sıfırlandı." : err;
            return RedirectToAction(nameof(Edit), new { id = dto.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive([FromBody] UserUpdateDTO dto)
        {
            var (ok, err) = await _users.ToggleLockAsync(dto.Id, dto.IsActive);
            if (!ok) return BadRequest(err);
            return Ok();
        }
    }
}
