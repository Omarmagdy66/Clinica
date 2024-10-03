using System.ComponentModel.DataAnnotations;

namespace Cls.Api.Dto
{
    public class BillingDto
    {
        public int? PatientId { get; set; }

        public int? AppointmentId { get; set; }
        public double? TotalAmount { get; set; }
        public double? PaidAmount { get; set; }
        public double? DueAmount { get; set; }

        public DateOnly? PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }
    }
}
