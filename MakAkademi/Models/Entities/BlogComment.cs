using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakAkademi.Models.Entities
{
    /// <summary>Blog yazı yorumları</summary>
    public class BlogComment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Blog Yazısı")]
        public int BlogPostId { get; set; }

        [ForeignKey(nameof(BlogPostId))]
        public BlogPost BlogPost { get; set; } = null!;

        [Required, MaxLength(50)]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Yorum")]
        public string Comment { get; set; } = string.Empty;

        [Display(Name = "Onaylandı")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}
