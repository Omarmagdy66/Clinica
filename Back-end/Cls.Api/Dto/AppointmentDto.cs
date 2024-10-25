namespace Dto;

	public class AppointmentDto
	{
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string? PatientNameIN { get; set; }
    public string? PatientNumberIN { get; set; }
    public string? PatientEmailIN { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeOnly? TimeSlot { get; set; }
    public bool? Teleconsultation { get; set; }

}

