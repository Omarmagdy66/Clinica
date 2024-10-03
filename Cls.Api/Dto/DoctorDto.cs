using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.DTO
{
	public class DoctorDto
	{
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ClinicAddress { get; set; }
    }
}
