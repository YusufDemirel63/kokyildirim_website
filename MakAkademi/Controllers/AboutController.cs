using Microsoft.AspNetCore.Mvc;

namespace MakAkademi.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index() => View();
    }
}
