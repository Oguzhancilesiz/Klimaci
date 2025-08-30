using Klimaci.DTO;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class MediaController : Controller
    {
        private readonly IMediaService _svc;
        public MediaController(IMediaService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 48, [FromQuery] string? search = null)
        {
            var model = await _svc.PagedAsync(page, pageSize, search);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] IdDTO dto)
        {
            await _svc.Delete(dto.Id);
            TempData["ok"] = "Medya silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
