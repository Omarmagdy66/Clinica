
namespace Models;

public class Clinic
{
    public int Id { get; set; }
    public int? CityId { get; set; }
    public int? CountryId { get; set; }
    public string? ClinicName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Image { get; set; }
    public Country? Country { get; set; }
    public City? City { get; set; }

    public ICollection<DoctorCLinic>? DoctorCLinics { get; set; }
}