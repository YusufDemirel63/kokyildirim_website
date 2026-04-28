using MakAkademi.Models.Entities;

namespace MakAkademi.Services.Interfaces
{
    public interface ITestimonialService
    {
        Task<IEnumerable<Testimonial>> GetActiveTestimonialsAsync(int count = 0);
        Task<IEnumerable<Testimonial>> GetAllAsync();
        Task<Testimonial?> GetByIdAsync(int id);
        Task CreateAsync(Testimonial testimonial);
        Task UpdateAsync(Testimonial testimonial);
        Task DeleteAsync(int id);
        Task ToggleActiveAsync(int id);
    }
}
