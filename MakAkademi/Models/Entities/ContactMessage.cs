using System;
using System.ComponentModel.DataAnnotations;

namespace MakAkademi.Models.Entities
{
    /// <summary>İletişim formu mesajları</summary>
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Ad Soyad")]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(254)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [Display(Name = "Konu")]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mesaj")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Okundu")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Gönderilme Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
