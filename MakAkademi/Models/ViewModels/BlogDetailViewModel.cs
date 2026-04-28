using MakAkademi.Models.Entities;

namespace MakAkademi.Models.ViewModels
{
    public class BlogDetailViewModel
    {
        public BlogPost Post { get; set; } = null!;
        public IEnumerable<BlogComment> Comments { get; set; } = Enumerable.Empty<BlogComment>();
        public IEnumerable<BlogPost> OtherPosts { get; set; } = Enumerable.Empty<BlogPost>();
    }
}
