
using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public partial class clinicdbContext : DbContext
{
    public clinicdbContext()
    {
    }

    public clinicdbContext(DbContextOptions<clinicdbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<Billing> Billings { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Clinic> Clinics { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<Query> Queries { get; set; }
    public virtual DbSet<Specialization> Specializations { get; set; }
    public virtual DbSet<DoctorSchedule> Schedules { get; set; }
    public virtual DbSet<Nurse> Nurses { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<DoctorCLinic> DoctorCLinics { get; set; }

}
