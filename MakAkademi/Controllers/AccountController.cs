using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MakAkademi.Controllers
{
    /// <summary>Admin giriş/çıkış (ASP.NET Core Identity)</summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("yonetim/giris")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("yonetim/giris")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return LocalRedirect(returnUrl ?? "/Admin/Dashboard");
                }

                // Admin rolü yoksa çıkış yap
                await _signInManager.SignOutAsync();
                ModelState.AddModelError("", "Bu hesabın admin yetkisi bulunmamaktadır.");
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            }

            return View();
        }

        [HttpPost]
        [Route("yonetim/cikis")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction(nameof(Login));
        }

        /// <summary>Tek seferlik admin kurulum endpoint'i</summary>
        [HttpGet]
        [Route("setup-admin")]
        public async Task<IActionResult> SetupAdmin([FromServices] IConfiguration config, string username, string email, string password, string secret)
        {
            var expectedSecret = config["AppSettings:SetupSecretKey"];
            if (string.IsNullOrEmpty(expectedSecret) || secret != expectedSecret)
                return Content("❌ Geçersiz secret key!", "text/plain");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return Content("❌ Eksik parametreler! Kullanım: /setup-admin?username=X&email=X&password=X&secret=X", "text/plain");

            var existing = await _userManager.FindByNameAsync(username);
            if (existing != null)
                return Content($"⚠️ '{username}' kullanıcısı zaten mevcut!", "text/plain");

            var user = new IdentityUser { UserName = username, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Content($"❌ Hata: {string.Join(", ", result.Errors.Select(e => e.Description))}", "text/plain");

            await _userManager.AddToRoleAsync(user, "Admin");

            return Content($"✅ Admin kullanıcı oluşturuldu! Kullanıcı: {username} | Giriş: /yonetim/giris", "text/plain");
        }
    }
}
