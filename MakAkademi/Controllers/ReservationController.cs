using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITimeSlotService _timeSlotService;

        public ReservationController(IReservationService reservationService, ITimeSlotService timeSlotService)
        {
            _reservationService = reservationService;
            _timeSlotService = timeSlotService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            string name, string email, string phone,
            EducationLevel educationLevel, ReservationLessonType lessonType,
            int? selectedSlotId, string? message)
        {
            var reservation = new ReservationRequest
            {
                Name = name,
                Email = email,
                Phone = phone,
                EducationLevel = educationLevel,
                LessonType = lessonType,
                SelectedSlotId = selectedSlotId,
                Message = message ?? string.Empty
            };

            await _reservationService.CreateAsync(reservation);
            TempData["Success"] = "Ders talebiniz başarıyla alındı! En kısa sürede sizinle iletişime geçeceğim.";
            return RedirectToAction(nameof(Index));
        }
    }
}
