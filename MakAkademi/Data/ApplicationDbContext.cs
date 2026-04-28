using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MakAkademi.Models.Entities;

namespace MakAkademi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AvailableTimeSlot> AvailableTimeSlots { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }
        public DbSet<PageView> PageViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Testimonial
            builder.Entity<Testimonial>(e =>
            {
                e.HasIndex(t => t.IsActive);
            });

            // BlogPost
            builder.Entity<BlogPost>(e =>
            {
                e.HasIndex(b => b.Slug).IsUnique();
                e.HasIndex(b => b.IsPublished);
                e.HasMany(b => b.Comments)
                 .WithOne(c => c.BlogPost)
                 .HasForeignKey(c => c.BlogPostId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // BlogComment
            builder.Entity<BlogComment>(e =>
            {
                e.HasIndex(c => c.IsApproved);
            });

            // AvailableTimeSlot - benzersiz Date+StartTime
            builder.Entity<AvailableTimeSlot>(e =>
            {
                e.HasIndex(s => new { s.Date, s.StartTime }).IsUnique();
            });

            // ReservationRequest
            builder.Entity<ReservationRequest>(e =>
            {
                e.HasOne(r => r.SelectedSlot)
                 .WithMany()
                 .HasForeignKey(r => r.SelectedSlotId)
                 .OnDelete(DeleteBehavior.SetNull);
                e.HasIndex(r => r.Status);
            });

            // PageView
            builder.Entity<PageView>(e =>
            {
                e.HasIndex(p => p.CreatedAt);
                e.HasIndex(p => new { p.SessionKey, p.CreatedAt });
            });
        }
    }
}
