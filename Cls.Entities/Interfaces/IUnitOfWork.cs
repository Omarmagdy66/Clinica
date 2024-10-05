
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
    public IRepository<Clinic> Clinics { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Specialization> Specializations { get; }
    public IRepository<Query> Queries { get; }
    public IRepository<DoctorSchedule> Schedules { get; }
    public IRepository<Nurse> Nurses { get; }
    public IRepository<Country> Countries { get; }
    public IRepository<City> Cities { get; }
                                             
                                             
    int Save();                              
}                                            
                                             