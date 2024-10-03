
using Models;

namespace  Interfaces;

public interface IUnitOfWork:IDisposable
{
    public IRepository<User> Users { get;  }
    public IRepository<Appointment> Appointments { get; }
    public IRepository<Billing> Billings { get; }
    public IRepository<Doctor> Doctors { get; }
    public IRepository<Review> Reviews { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Clinic> Locations { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Specialization> Specializations { get; }
    public IRepository<Query> Queries { get; }
    public IRepository<DoctorSchedule> Schedules { get; }

    int Save();
}
