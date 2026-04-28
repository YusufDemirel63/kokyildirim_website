using System;
using System.ComponentModel.DataAnnotations;

namespace MakAkademi.Models.Entities
{
    public enum LessonType
    {
        [Display(Name = "Online")]
        Online,
        [Display(Name = "Yüz Yüze")]
        Yuzyuze,
        [Display(Name = "Her İkisi")]
        Both
    }

    /// <summary>Öğretmenin müsait olduğu zaman dilimleri</summary>
    public class AvailableTimeSlot
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tarih")]
        public DateOnly Date { get; set; }

        [Required]
        [Display(Name = "Başlangıç Saati")]
        public TimeOnly StartTime { get; set; }

        [Required]
        [Display(Name = "Bitiş Saati")]
        public TimeOnly EndTime { get; set; }

        [Display(Name = "Müsait")]
        public bool IsAvailable { get; set; } = true;

        [Required]
        [Display(Name = "Ders Türü")]
        public LessonType LessonType { get; set; } = LessonType.Both;

        [Display(Name = "Notlar")]
        public string Notes { get; set; } = string.Empty;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsPast()
        {
            var slotDateTime = Date.ToDateTime(StartTime);
            return slotDateTime < DateTime.Now;
        }
    }
}
