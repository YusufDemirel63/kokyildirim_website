using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class ReservationsController : AdminBaseController
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string status = "all")
        {
            ReservationStatus? filter = status == "all" ? null :
                Enum.TryParse<ReservationStatus>(status, true, out var s) ? s : null;

            var reservations = await _service.GetAllAsync(filter);
            ViewBag.StatusFilter = status;
            return View(reservations);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ReservationStatus status)
        {
            await _service.UpdateStatusAsync(id, status);
            TempData["Success"] = "Rezervasyon durumu güncellendi!";
            return RedirectToAction(nameof(Index));
        }
    }
}
