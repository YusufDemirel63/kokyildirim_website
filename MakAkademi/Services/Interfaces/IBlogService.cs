using MakAkademi.Models.Entities;

namespace MakAkademi.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogPost>> GetPublishedPostsAsync(int page = 1, int pageSize = 6);
        Task<int> GetPublishedCountAsync();
        Task<BlogPost?> GetBySlugAsync(string slug);
        Task<BlogPost?> GetByIdAsync(int id);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<IEnumerable<BlogPost>> GetRelatedPostsAsync(int excludeId, int count = 3);
        Task<BlogPost> CreateAsync(BlogPost post);
        Task UpdateAsync(BlogPost post);
        Task DeleteAsync(int id);
        Task TogglePublishAsync(int id);
        Task AddCommentAsync(BlogComment comment);
        Task<IEnumerable<BlogComment>> GetApprovedCommentsAsync(int postId);
    }
}
