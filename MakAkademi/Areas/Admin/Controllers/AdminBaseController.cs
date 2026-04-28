using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakAkademi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : Controller
    {
        // Tüm admin controller'ları bu base class'tan türer.
        // [Area("Admin")] ve [Authorize(Roles = "Admin")] otomatik uygulanır.
    }
}
