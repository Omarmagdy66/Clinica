
using System.ComponentModel.DataAnnotations;

namespace Models;

public class Nurse
{
    public int Id { get; set; }
    public int? ClinicId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    [DataType("decimal(8,2)")]
    public double? Price { get; set; }

    public Clinic? clinic { get; set; }
}
