namespace Dto;

public class DoctorScheduleDto
{
    public int? ClinicId { get; set; }
    public DateTime? Day { get; set; }
    public TimeOnly? AvailableFrom { get; set; }
    public TimeOnly? AvailableTo { get; set; }
    }
