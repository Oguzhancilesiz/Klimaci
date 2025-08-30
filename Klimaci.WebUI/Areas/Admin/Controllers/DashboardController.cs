using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IAdminDashboardService _svc;
        public DashboardController(IAdminDashboardService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int days = 30)
        {
            var vm = await _svc.GetAsync(days);
            ViewBag.CurrentDays = days;
            return View(vm);
        }

    }
}
