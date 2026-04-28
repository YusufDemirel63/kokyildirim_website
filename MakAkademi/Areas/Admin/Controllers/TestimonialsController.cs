using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class TestimonialsController : AdminBaseController
    {
        private readonly ITestimonialService _service;

        public TestimonialsController(ITestimonialService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var testimonials = await _service.GetAllAsync();
            return View(testimonials);
        }

        [HttpGet]
        public IActionResult Create() => View("Form", new Testimonial { Rating = 5, IsActive = true });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            await _service.CreateAsync(model);
            TempData["Success"] = "Yorum başarıyla oluşturuldu!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View("Form", item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Testimonial model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            model.Id = id;
            await _service.UpdateAsync(model);
            TempData["Success"] = "Yorum başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Yorum başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleActiveAsync(id);
            TempData["Success"] = "Yorum durumu güncellendi!";
            return RedirectToAction(nameof(Index));
        }
    }
}
