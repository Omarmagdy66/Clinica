
namespace Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string PatientNameIN { get; set; }
    public string PatientNumberIN { get; set; }
    public string PatientEmailIN { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string? Status { get; set; } // "booked", "cancelled", "completed"
    public bool? Teleconsultation { get; set; }

    public Patient? Patient { get; set; } 
    public Doctor? Doctor { get; set; } 
}
