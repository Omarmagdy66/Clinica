
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
[PrimaryKey(nameof(DoctorId), nameof(ClinicId))]
public class DoctorCLinic
{
    public int DoctorId { get; set; }
    public int ClinicId { get; set; }

    public Clinic? Clinic { get; set; }
    public Doctor? Doctor { get; set; }
}
