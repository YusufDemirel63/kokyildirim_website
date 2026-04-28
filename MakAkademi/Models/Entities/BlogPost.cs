using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakAkademi.Models.Entities
{
    /// <summary>Blog yazıları</summary>
    public class BlogPost
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [Display(Name = "URL (Slug)")]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [MaxLength(300)]
        [Display(Name = "Özet")]
        public string Excerpt { get; set; } = string.Empty;

        [Display(Name = "Görsel")]
        public string? ImagePath { get; set; }

        [Display(Name = "Yayında")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
    }
}
