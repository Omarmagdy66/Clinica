namespace Dto;

	public class AppointmentDto
	{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string? PatientNameIN { get; set; }
    public string? PatientNumberIN { get; set; }
    public string? PatientEmailIN { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeSpan TimeSlot { get; set; }
    public string? Status { get; set; } // "booked", "cancelled", "completed"
    public bool? Teleconsultation { get; set; }


}

