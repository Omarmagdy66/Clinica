
using System.ComponentModel.DataAnnotations;

namespace Models;

public class Doctor
{
    public int Id { get; set; }
    public int? SpecializationId { get; set; }
    public int? Examinationduration { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    [DataType("decimal(8,2)")]
    public double? Price { get; set; }
    public Specialization? specialization { get; set; }
    public virtual ICollection<DoctorCLinic>? DoctorCLinics { get; set; }
}
