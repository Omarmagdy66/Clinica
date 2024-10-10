

namespace Models
{
    public class adminRequest
    {
        public int Id { get; set; }
        public string? ClinicName { get; set; }

        public string? Address { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string? PhoneNumber { get; set; }
        public int DoctorId { get; set; }    
        public Country? Country { get; set; }
        public Doctor? Doctor { get; set; }
        public City? City { get; set; }
    }
}
