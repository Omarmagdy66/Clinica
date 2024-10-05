namespace Dto;

public class DoctorScheduleDto
{
    public int? DoctorId { get; set; }
    public int? ClinicId { get; set; }
    public DateOnly? Day { get; set; }
    public TimeSpan? AvailableFrom { get; set; }
    public TimeSpan? AvailableTo { get; set; }
    public bool? Status { get; set; }
}
