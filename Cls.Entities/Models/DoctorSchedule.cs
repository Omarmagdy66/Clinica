
namespace Models;
public class DoctorSchedule
{
    public int? Id { get; set; }
    public int? DoctorId { get; set; }
    public int? ClinicId { get; set; }
    public DateOnly? Day { get; set; }
    public TimeSpan? AvailableFrom { get; set; }
    public TimeSpan? AvailableTo { get; set; }
    public bool? Status { get; set; } // 0 booked   1 available
    public Doctor? Doctor { get; set; }
    public Clinic? Clinic { get; set; }

}