using MakAkademi.Models.Entities;

namespace MakAkademi.Services.Interfaces
{
    public interface IContactService
    {
        Task SaveMessageAsync(ContactMessage message);
        Task<IEnumerable<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
        Task<int> GetUnreadCountAsync();
    }
}
