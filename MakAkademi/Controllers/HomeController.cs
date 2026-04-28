using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Models.ViewModels;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public HomeController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public async Task<IActionResult> Index()
        {
            var testimonials = await _testimonialService.GetActiveTestimonialsAsync(3);
            var vm = new HomeViewModel { Testimonials = testimonials };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
