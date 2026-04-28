using Microsoft.AspNetCore.Mvc;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class MessagesController : AdminBaseController
    {
        private readonly IContactService _service;

        public MessagesController(IContactService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _service.GetAllAsync();
            return View(messages);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var message = await _service.GetByIdAsync(id);
            if (message == null) return NotFound();

            await _service.MarkAsReadAsync(id);
            return View(message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Mesaj başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }
    }
}
