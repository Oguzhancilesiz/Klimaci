using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Controllers
{
    [AllowAnonymous]
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _svc;
        public NewsletterController(INewsletterService svc) { _svc = svc; }

        // GET /newsletter/confirm?id=...&token=...
        [HttpGet("/newsletter/confirm")]
        public async Task<IActionResult> Confirm([FromQuery] Guid id, [FromQuery] string token)
        {
            var (ok, error) = await _svc.ConfirmAsync(id, token);
            ViewBag.Ok = ok;
            ViewBag.Error = error;
            return View("Confirm"); // Views/Newsletter/Confirm.cshtml
        }

        // GET /newsletter/unsubscribe?id=...
        [HttpGet("/newsletter/unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromQuery] Guid id)
        {
            var (ok, error) = await _svc.UnsubscribeAsync(id);
            ViewBag.Ok = ok;
            ViewBag.Error = error;
            return View("Unsubscribe");
        }
    }
}
