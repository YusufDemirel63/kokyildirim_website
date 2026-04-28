using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class TimeSlotsController : AdminBaseController
    {
        private readonly ITimeSlotService _service;

        public TimeSlotsController(ITimeSlotService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var slots = await _service.GetAllAsync();
            return View(slots);
        }

        [HttpGet]
        public IActionResult Create() => View("Form", new AvailableTimeSlot { IsAvailable = true, LessonType = LessonType.Both });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvailableTimeSlot model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            await _service.CreateAsync(model);
            TempData["Success"] = "Zaman dilimi başarıyla oluşturuldu!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slot = await _service.GetByIdAsync(id);
            if (slot == null) return NotFound();
            return View("Form", slot);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AvailableTimeSlot model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            model.Id = id;
            await _service.UpdateAsync(model);
            TempData["Success"] = "Zaman dilimi başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Zaman dilimi başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }
    }
}
