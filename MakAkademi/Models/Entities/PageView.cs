using System;
using System.ComponentModel.DataAnnotations;

namespace MakAkademi.Models.Entities
{
    /// <summary>Sayfa ziyaret istatistikleri</summary>
    public class PageView
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        [Display(Name = "Sayfa URL")]
        public string Path { get; set; } = string.Empty;

        [MaxLength(45)]
        [Display(Name = "IP Adresi")]
        public string? IpAddress { get; set; }

        [Display(Name = "User Agent")]
        public string UserAgent { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Referrer")]
        public string Referrer { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Session")]
        public string SessionKey { get; set; } = string.Empty;

        [Display(Name = "Ziyaret Zamanı")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
