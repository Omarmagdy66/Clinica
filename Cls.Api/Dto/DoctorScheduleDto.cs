namespace Cls.Api.Dto
{
    public class DoctorScheduleDto
    {
        public int DoctorId { get; set; }
        public string? DayOfWeek { get; set; } // e.g., "Monday", "Tuesday"
        public string? WorkingHours { get; set; } // e.g., "9-5"
    }
}
