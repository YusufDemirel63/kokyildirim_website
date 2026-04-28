using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index() => View(new ContactMessage());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessage model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _contactService.SaveMessageAsync(model);
            TempData["Success"] = "Mesajınız başarıyla gönderildi! En kısa sürede size dönüş yapacağım.";
            return RedirectToAction(nameof(Index));
        }
    }
}
