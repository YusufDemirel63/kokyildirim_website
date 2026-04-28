using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetPublishedPostsAsync(int page = 1, int pageSize = 6)
            => await _context.BlogPosts
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<int> GetPublishedCountAsync()
            => await _context.BlogPosts.CountAsync(b => b.IsPublished);

        public async Task<BlogPost?> GetBySlugAsync(string slug)
            => await _context.BlogPosts
                .Include(b => b.Comments.Where(c => c.IsApproved))
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);

        public async Task<BlogPost?> GetByIdAsync(int id)
            => await _context.BlogPosts.FindAsync(id);

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
            => await _context.BlogPosts
                .Include(b => b.Comments)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<BlogPost>> GetRelatedPostsAsync(int excludeId, int count = 3)
            => await _context.BlogPosts
                .Where(b => b.IsPublished && b.Id != excludeId)
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .ToListAsync();

        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            post.CreatedAt = DateTime.Now;
            post.UpdatedAt = DateTime.Now;
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task UpdateAsync(BlogPost post)
        {
            post.UpdatedAt = DateTime.Now;
            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post != null)
            {
                _context.BlogPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TogglePublishAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post != null)
            {
                post.IsPublished = !post.IsPublished;
                post.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCommentAsync(BlogComment comment)
        {
            comment.CreatedAt = DateTime.Now;
            comment.IsApproved = false;
            _context.BlogComments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogComment>> GetApprovedCommentsAsync(int postId)
            => await _context.BlogComments
                .Where(c => c.BlogPostId == postId && c.IsApproved)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
    }
}
