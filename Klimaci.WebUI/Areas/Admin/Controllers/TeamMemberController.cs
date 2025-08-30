using Klimaci.DTO.TeamMemberDTOs;
using Klimaci.DTO;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberService _svc;
        public TeamMemberController(ITeamMemberService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] bool desc = false)
            => View(await _svc.PagedAsync(new ListQueryDTO(page, pageSize, search, sortBy, desc)));

        [HttpGet]
        public IActionResult Create() => View(new TeamMemberAddDTO(
            Name: "", TitleText: null, Bio: null, Email: null, Phone: null,
            Slug: null, SeoTitle: null, SeoDescription: null,
            DisplayOrder: 0, IsPublished: true));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMemberAddDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.Add(dto); TempData["ok"] = "Eklendi."; return RedirectToAction(nameof(Index)); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e is null) return NotFound();
            return View(new TeamMemberUpdateDTO(e.Id, e.Name, e.TitleText, e.Bio, e.Email, e.Phone, e.Slug, e.SeoTitle, e.SeoDescription, e.DisplayOrder, e.IsPublished));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamMemberUpdateDTO dto)
        { if (!ModelState.IsValid) return View(dto); await _svc.Update(dto); TempData["ok"] = "Güncellendi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        { await _svc.Delete(id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }

        [HttpPost]
        public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto)
            => Json(await _svc.SlugCheckAsync(dto));

        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO body)
        { await _svc.ReorderAsync(body.Items); return Ok(); }
    }
}
