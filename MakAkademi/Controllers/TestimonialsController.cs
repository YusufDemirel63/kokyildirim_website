using Microsoft.AspNetCore.Mvc;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ITestimonialService _service;
        private const int PageSize = 6;

        public TestimonialsController(ITestimonialService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var all = await _service.GetActiveTestimonialsAsync();
            var list = all.ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(list.Count / (double)PageSize);

            var paged = list.Skip((page - 1) * PageSize).Take(PageSize);
            return View(paged);
        }
    }
}
