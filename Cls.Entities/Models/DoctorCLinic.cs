
namespace Models;

public class DoctorCLinic
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int ClinicId { get; set; }

    public Clinic Clinic { get; set; }
    public Doctor Doctor { get; set; }
}
