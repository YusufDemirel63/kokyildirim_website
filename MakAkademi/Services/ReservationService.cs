using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationRequest> CreateAsync(ReservationRequest reservation)
        {
            reservation.CreatedAt = DateTime.Now;
            reservation.Status = ReservationStatus.Beklemede;

            // Seçilen slot varsa rezerve et
            if (reservation.SelectedSlotId.HasValue)
            {
                var slot = await _context.AvailableTimeSlots.FindAsync(reservation.SelectedSlotId.Value);
                if (slot != null)
                {
                    reservation.SelectedDate = slot.Date;
                    reservation.SelectedTime = slot.StartTime;
                    slot.IsAvailable = false;
                }
            }

            _context.ReservationRequests.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<IEnumerable<ReservationRequest>> GetAllAsync(ReservationStatus? status = null)
        {
            var query = _context.ReservationRequests
                .Include(r => r.SelectedSlot)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task<ReservationRequest?> GetByIdAsync(int id)
            => await _context.ReservationRequests
                .Include(r => r.SelectedSlot)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task UpdateStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _context.ReservationRequests.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetPendingCountAsync()
            => await _context.ReservationRequests.CountAsync(r => r.Status == ReservationStatus.Beklemede);

        public async Task<int> GetThisMonthCountAsync()
        {
            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _context.ReservationRequests.CountAsync(r => r.CreatedAt >= firstDay);
        }
    }
}
