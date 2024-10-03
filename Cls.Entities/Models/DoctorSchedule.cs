

using System.ComponentModel.DataAnnotations;

namespace Models;
public class DoctorSchedule
{
    public int? Id { get; set; }
    public int? DoctorId { get; set; }
    public DateOnly? Day { get; set; }
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }
    public string? Status { get; set; }
    public Doctor? Doctor { get; set; }

}
