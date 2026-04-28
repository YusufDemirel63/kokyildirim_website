using Microsoft.AspNetCore.Mvc;
using MakAkademi.Models.Entities;
using MakAkademi.Services.Interfaces;

namespace MakAkademi.Controllers
{
    /// <summary>Rezervasyon takvimi için JSON API endpoint</summary>
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;

        public ApiController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        // GET /api/available-slots?lesson_type=online
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery(Name = "lesson_type")] string? lessonTypeParam)
        {
            LessonType? lessonType = null;

            if (!string.IsNullOrEmpty(lessonTypeParam) && lessonTypeParam != "farketmez")
            {
                lessonType = lessonTypeParam.ToLower() switch
                {
                    "online" => LessonType.Online,
                    "yuzyuze" => LessonType.Yuzyuze,
                    _ => null
                };
            }

            var slots = await _timeSlotService.GetAvailableSlotsAsync(lessonType);

            var result = slots.Select(s => new
            {
                id = s.Id,
                date = s.Date.ToString("yyyy-MM-dd"),
                start_time = s.StartTime.ToString("HH:mm"),
                end_time = s.EndTime.ToString("HH:mm"),
                lesson_type = s.LessonType.ToString().ToLower(),
                notes = s.Notes
            });

            return Ok(new { slots = result });
        }
    }
}
