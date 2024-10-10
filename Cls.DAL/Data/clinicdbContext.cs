
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
    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<adminRequest> AdminRequests { get; set; }
    public virtual DbSet<NurseAdminRequest> NusreAdminRequests { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<NurseNotification> NurseNotifications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorCLinic>()
            .HasKey(dc => new { dc.DoctorId, dc.ClinicId });

        modelBuilder.Entity<DoctorCLinic>()
            .HasOne(dc => dc.Doctor)
            .WithMany(d => d.DoctorCLinics)
            .HasForeignKey(dc => dc.DoctorId);

        modelBuilder.Entity<DoctorCLinic>()
            .HasOne(dc => dc.Clinic)
            .WithMany(c => c.DoctorCLinics)
            .HasForeignKey(dc => dc.ClinicId);
    }


}
