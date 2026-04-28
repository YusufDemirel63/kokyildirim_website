using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ApplicationDbContext _context;

        public TimeSlotService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AvailableTimeSlot>> GetAvailableSlotsAsync(LessonType? lessonType = null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var endDate = today.AddDays(60);
            var nowTime = TimeOnly.FromDateTime(DateTime.Now);

            var query = _context.AvailableTimeSlots
                .Where(s => s.IsAvailable
                    && s.Date >= today
                    && s.Date <= endDate
                    && !(s.Date == today && s.StartTime < nowTime));

            if (lessonType.HasValue && lessonType.Value != LessonType.Both)
            {
                query = query.Where(s => s.LessonType == lessonType.Value || s.LessonType == LessonType.Both);
            }

            return await query.OrderBy(s => s.Date).ThenBy(s => s.StartTime).ToListAsync();
        }

        public async Task<IEnumerable<AvailableTimeSlot>> GetAllAsync()
            => await _context.AvailableTimeSlots
                .OrderByDescending(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

        public async Task<AvailableTimeSlot?> GetByIdAsync(int id)
            => await _context.AvailableTimeSlots.FindAsync(id);

        public async Task CreateAsync(AvailableTimeSlot slot)
        {
            slot.CreatedAt = DateTime.Now;
            _context.AvailableTimeSlots.Add(slot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AvailableTimeSlot slot)
        {
            _context.AvailableTimeSlots.Update(slot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slot = await _context.AvailableTimeSlots.FindAsync(id);
            if (slot != null)
            {
                _context.AvailableTimeSlots.Remove(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetFutureAvailableCountAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.AvailableTimeSlots
                .CountAsync(s => s.Date >= today && s.IsAvailable);
        }
    }
}
