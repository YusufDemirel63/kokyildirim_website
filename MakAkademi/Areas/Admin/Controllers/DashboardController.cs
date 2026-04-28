using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakAkademi.Areas.Admin.Controllers;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationService _reservationService;
        private readonly IContactService _contactService;
        private readonly ITimeSlotService _timeSlotService;
        private readonly ITestimonialService _testimonialService;

        public DashboardController(
            ApplicationDbContext context,
            IReservationService reservationService,
            IContactService contactService,
            ITimeSlotService timeSlotService,
            ITestimonialService testimonialService)
        {
            _context = context;
            _reservationService = reservationService;
            _contactService = contactService;
            _timeSlotService = timeSlotService;
            _testimonialService = testimonialService;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var thisMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastWeek = DateTime.Now.AddDays(-7);
            var yesterday = DateTime.Now.AddDays(-1);

            // Ziyaretçi istatistikleri
            var totalViews = await _context.PageViews.CountAsync();
            var todayViews = await _context.PageViews.CountAsync(p => p.CreatedAt.Date == DateTime.Today);
            var yesterdayViews = await _context.PageViews.CountAsync(p => p.CreatedAt.Date == DateTime.Today.AddDays(-1));
            var weekViews = await _context.PageViews.CountAsync(p => p.CreatedAt >= lastWeek);
            var monthViews = await _context.PageViews.CountAsync(p => p.CreatedAt >= thisMonthStart);

            var uniqueToday = await _context.PageViews
                .Where(p => p.CreatedAt.Date == DateTime.Today)
                .Select(p => p.SessionKey).Distinct().CountAsync();

            var uniqueMonth = await _context.PageViews
                .Where(p => p.CreatedAt >= thisMonthStart)
                .Select(p => p.SessionKey).Distinct().CountAsync();

            var popularPages = await _context.PageViews
                .Where(p => p.CreatedAt >= thisMonthStart)
                .GroupBy(p => p.Path)
                .Select(g => new { Path = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewBag.Stats = new
            {
                TotalReservations = await _context.ReservationRequests.CountAsync(),
                PendingReservations = await _reservationService.GetPendingCountAsync(),
                TotalSlots = await _context.AvailableTimeSlots.CountAsync(s => s.Date >= today),
                AvailableSlots = await _timeSlotService.GetFutureAvailableCountAsync(),
                TotalTestimonials = await _context.Testimonials.CountAsync(),
                ActiveTestimonials = await _context.Testimonials.CountAsync(t => t.IsActive),
                UnreadMessages = await _contactService.GetUnreadCountAsync(),
                ThisMonthReservations = await _reservationService.GetThisMonthCountAsync(),
                TotalViews = totalViews,
                TodayViews = todayViews,
                YesterdayViews = yesterdayViews,
                WeekViews = weekViews,
                MonthViews = monthViews,
                UniqueVisitorsToday = uniqueToday,
                UniqueVisitorsMonth = uniqueMonth,
                PopularPages = popularPages
            };

            ViewBag.RecentReservations = await _context.ReservationRequests
                .OrderByDescending(r => r.CreatedAt).Take(5).ToListAsync();
            ViewBag.RecentMessages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt).Take(5).ToListAsync();
            ViewBag.UpcomingSlots = await _context.AvailableTimeSlots
                .Where(s => s.Date >= today && !s.IsAvailable)
                .OrderBy(s => s.Date).ThenBy(s => s.StartTime).Take(5).ToListAsync();

            return View();
        }
    }
}
