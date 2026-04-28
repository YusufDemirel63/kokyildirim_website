using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(ContactMessage message)
        {
            message.CreatedAt = DateTime.Now;
            message.IsRead = false;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
            => await _context.ContactMessages.OrderByDescending(m => m.CreatedAt).ToListAsync();

        public async Task<ContactMessage?> GetByIdAsync(int id)
            => await _context.ContactMessages.FindAsync(id);

        public async Task MarkAsReadAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null && !msg.IsRead)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                _context.ContactMessages.Remove(msg);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync()
            => await _context.ContactMessages.CountAsync(m => !m.IsRead);
    }
}
