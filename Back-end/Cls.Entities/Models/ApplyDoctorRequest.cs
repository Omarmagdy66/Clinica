using System.ComponentModel.DataAnnotations;


namespace Models;

public class ApplyDoctorRequest
{

    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int? SpecializationId { get; set; }
    public int? Examinationduration { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    [DataType("decimal(8,2)")]
    public double? Price { get; set; }

    public Specialization? Specialization { get; set; }
    public Doctor? Doctor { get; set; }
}
