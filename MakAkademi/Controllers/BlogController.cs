using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Models.ViewModels;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private const int PageSize = 6;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var posts = await _blogService.GetPublishedPostsAsync(page, PageSize);
            var totalCount = await _blogService.GetPublishedCountAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            return View(posts);
        }

        [Route("blog/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var post = await _blogService.GetBySlugAsync(slug);
            if (post == null) return NotFound();

            var otherPosts = await _blogService.GetRelatedPostsAsync(post.Id, 3);

            var vm = new BlogDetailViewModel
            {
                Post = post,
                Comments = post.Comments.Where(c => c.IsApproved).OrderByDescending(c => c.CreatedAt).ToList(),
                OtherPosts = otherPosts
            };

            return View(vm);
        }

        [HttpPost]
        [Route("blog/{slug}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(string slug, string firstName, string lastName, string comment)
        {
            var post = await _blogService.GetBySlugAsync(slug);
            if (post == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(comment))
            {
                await _blogService.AddCommentAsync(new BlogComment
                {
                    BlogPostId = post.Id,
                    FirstName = firstName.Trim(),
                    LastName = lastName.Trim(),
                    Comment = comment.Trim()
                });
                TempData["CommentSuccess"] = "✅ Yorumunuz başarıyla gönderildi! Onaylandıktan sonra yayınlanacaktır.";
            }

            return RedirectToAction(nameof(Detail), new { slug });
        }
    }
}
