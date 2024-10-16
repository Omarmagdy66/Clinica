
namespace Dto;

	public class PatientDto
	{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? RegistrationDate { get; set; } = DateTime.Now.Date;
}
