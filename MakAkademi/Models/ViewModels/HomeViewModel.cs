using MakAkademi.Models.Entities;

namespace MakAkademi.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Testimonial> Testimonials { get; set; } = Enumerable.Empty<Testimonial>();
    }
}
