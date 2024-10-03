
namespace Models;

public partial class Review
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateOnly? ReviewDate { get; set; }

    public Patient? Patient { get; set; } 
    public Doctor? Doctor { get; set; } 
}
