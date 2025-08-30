using Klimaci.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : Controller
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IEFContext _ctx;
        protected AdminBaseController(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        protected void OkAlert(string m) => TempData["ok"] = m;
        protected void ErrAlert(string m) => TempData["err"] = m;
    }
}
