using System;
using System.ComponentModel.DataAnnotations;

namespace MakAkademi.Models.Entities
{
    /// <summary>Öğrenci yorumları</summary>
    public class Testimonial
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Öğrenci Adı")]
        public string StudentName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        [Display(Name = "Eğitim Seviyesi")]
        public string StudentLevel { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Yorum")]
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5)]
        [Display(Name = "Puan (1-5)")]
        public int Rating { get; set; } = 5;

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
