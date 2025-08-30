using Klimaci.DTO.Auth;
using Klimaci.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // sadece bu controller login için anonim
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signIn;
        private readonly UserManager<AppUser> _userMgr;

        public AccountController(SignInManager<AppUser> signIn, UserManager<AppUser> userMgr)
        {
            _signIn = signIn;
            _userMgr = userMgr;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new AdminLoginDTO { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["err"] = "E-posta ve şifre zorunlu.";
                return View(dto);
            }

            var user = await _userMgr.FindByEmailAsync(dto.Email.Trim());
            if (user is null)
            {
                TempData["err"] = "Kullanıcı bulunamadı.";
                return View(dto);
            }

            // Kullanıcının Admin rolü var mı?
            var isAdmin = await _userMgr.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                TempData["err"] = "Bu panel için Admin yetkisi gerekli.";
                return View(dto);
            }

            var result = await _signIn.PasswordSignInAsync(user, dto.Password, dto.RememberMe, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    TempData["err"] = "Hesap geçici olarak kilitlendi. Lütfen sonra tekrar deneyin.";
                }
                else
                {
                    TempData["err"] = "Giriş başarısız. Bilgileri kontrol edin.";
                }
                return View(dto);
            }

            TempData["ok"] = "Hoş geldin.";
            if (!string.IsNullOrWhiteSpace(dto.ReturnUrl) && Url.IsLocalUrl(dto.ReturnUrl))
                return Redirect(dto.ReturnUrl);

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            TempData["ok"] = "Çıkış yapıldı.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
    }
}
