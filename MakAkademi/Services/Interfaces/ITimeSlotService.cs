using MakAkademi.Models.Entities;

namespace MakAkademi.Services.Interfaces
{
    public interface ITimeSlotService
    {
        Task<IEnumerable<AvailableTimeSlot>> GetAvailableSlotsAsync(LessonType? lessonType = null);
        Task<IEnumerable<AvailableTimeSlot>> GetAllAsync();
        Task<AvailableTimeSlot?> GetByIdAsync(int id);
        Task CreateAsync(AvailableTimeSlot slot);
        Task UpdateAsync(AvailableTimeSlot slot);
        Task DeleteAsync(int id);
        Task<int> GetFutureAvailableCountAsync();
    }
}
