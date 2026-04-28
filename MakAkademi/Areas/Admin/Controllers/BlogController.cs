using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class BlogController : AdminBaseController
    {
        private readonly IBlogService _service;
        private readonly IWebHostEnvironment _env;

        public BlogController(IBlogService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _service.GetAllAsync();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Create() => View("Form", new BlogPost { IsPublished = true });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost model, IFormFile? image)
        {
            if (!ModelState.IsValid) return View("Form", model);

            if (image != null && image.Length > 0)
                model.ImagePath = await SaveImageAsync(image);

            await _service.CreateAsync(model);
            TempData["Success"] = "Blog yazısı başarıyla oluşturuldu!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound();
            return View("Form", post);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost model, IFormFile? image)
        {
            if (!ModelState.IsValid) return View("Form", model);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Title = model.Title;
            existing.Slug = model.Slug;
            existing.Content = model.Content;
            existing.Excerpt = model.Excerpt;
            existing.IsPublished = model.IsPublished;

            if (image != null && image.Length > 0)
                existing.ImagePath = await SaveImageAsync(image);

            await _service.UpdateAsync(existing);
            TempData["Success"] = "Blog yazısı başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Blog yazısı başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(int id)
        {
            await _service.TogglePublishAsync(id);
            TempData["Success"] = "Blog yazısı durumu güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "blog");
            Directory.CreateDirectory(uploadsDir);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            return $"/uploads/blog/{fileName}";
        }
    }
}
