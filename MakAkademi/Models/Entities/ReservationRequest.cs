using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakAkademi.Models.Entities
{
    public enum EducationLevel
    {
        [Display(Name = "İlkokul (4-5. Sınıf)")] Ilkokul,
        [Display(Name = "6. Sınıf")] Sinif6,
        [Display(Name = "7. Sınıf")] Sinif7,
        [Display(Name = "8. Sınıf (LGS)")] Sinif8,
        [Display(Name = "9. Sınıf")] Sinif9,
        [Display(Name = "10. Sınıf")] Sinif10,
        [Display(Name = "11. Sınıf")] Sinif11,
        [Display(Name = "12. Sınıf (TYT-AYT)")] Sinif12,
        [Display(Name = "Üniversite")] Universite
    }

    public enum ReservationLessonType
    {
        [Display(Name = "Online")] Online,
        [Display(Name = "Yüz Yüze")] Yuzyuze,
        [Display(Name = "Farketmez")] Farketmez
    }

    public enum ReservationStatus
    {
        [Display(Name = "Beklemede")] Beklemede,
        [Display(Name = "Onaylandı")] Onaylandi,
        [Display(Name = "Tamamlandı")] Tamamlandi,
        [Display(Name = "İptal Edildi")] Iptal
    }

    /// <summary>Ders rezervasyon talepleri</summary>
    public class ReservationRequest
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

        [Required]
        [Display(Name = "Eğitim Seviyesi")]
        public EducationLevel EducationLevel { get; set; }

        [Required]
        [Display(Name = "Ders Türü")]
        public ReservationLessonType LessonType { get; set; }

        // Seçilen zaman dilimi (FK)
        [Display(Name = "Seçilen Zaman Dilimi")]
        public int? SelectedSlotId { get; set; }

        [ForeignKey(nameof(SelectedSlotId))]
        public AvailableTimeSlot? SelectedSlot { get; set; }

        [Display(Name = "Seçilen Tarih")]
        public DateOnly? SelectedDate { get; set; }

        [Display(Name = "Seçilen Saat")]
        public TimeOnly? SelectedTime { get; set; }

        [Display(Name = "Uygun Gün ve Saatler")]
        public string AvailableDays { get; set; } = string.Empty;

        [Display(Name = "Ek Mesaj")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Durum")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Beklemede;

        [Display(Name = "Talep Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
