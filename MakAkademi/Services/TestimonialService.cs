using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Services
{
    public class TestimonialService : ITestimonialService
    {
        private readonly ApplicationDbContext _context;

        public TestimonialService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Testimonial>> GetActiveTestimonialsAsync(int count = 0)
        {
            var query = _context.Testimonials
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt);

            return count > 0
                ? await query.Take(count).ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<IEnumerable<Testimonial>> GetAllAsync()
            => await _context.Testimonials.OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task<Testimonial?> GetByIdAsync(int id)
            => await _context.Testimonials.FindAsync(id);

        public async Task CreateAsync(Testimonial testimonial)
        {
            testimonial.CreatedAt = DateTime.Now;
            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Testimonials.FindAsync(id);
            if (item != null)
            {
                _context.Testimonials.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleActiveAsync(int id)
        {
            var item = await _context.Testimonials.FindAsync(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                await _context.SaveChangesAsync();
            }
        }
    }
}
