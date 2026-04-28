using Microsoft.AspNetCore.Mvc;

namespace MakAkademi.Controllers
{
    public class LessonsController : Controller
    {
        public IActionResult Index() => View();
    }
}
