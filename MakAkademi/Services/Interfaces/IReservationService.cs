using MakAkademi.Models.Entities;

namespace MakAkademi.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationRequest> CreateAsync(ReservationRequest reservation);
        Task<IEnumerable<ReservationRequest>> GetAllAsync(ReservationStatus? status = null);
        Task<ReservationRequest?> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, ReservationStatus status);
        Task<int> GetPendingCountAsync();
        Task<int> GetThisMonthCountAsync();
    }
}
