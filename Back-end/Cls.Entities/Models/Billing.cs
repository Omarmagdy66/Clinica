
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Billing
{
    public int Id { get; set; }
    public int? PatientId { get; set; }
    public int? AppointmentId { get; set; }
    [DataType("decimal(8,2)")]
    public double? TotalAmount { get; set; }
    [DataType("decimal(8,2)")]
    public double? PaidAmount { get; set; }
    [DataType("decimal(8,2)")]
    public double? DueAmount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Patient? Patient { get; set; }
}
