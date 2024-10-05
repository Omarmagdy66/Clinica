using DAL;
using Data;
using Interfaces;
using Models;

namespace Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly clinicdbContext _context;

    public UnitOfWork(clinicdbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Roles = new Repository<Role>(_context);
        Patients = new Repository<Patient>(_context);
        Doctors = new Repository<Doctor>(_context);
        Appointments = new Repository<Appointment>(_context);
        Billings = new Repository<Billing>(_context);
        Queries = new Repository<Query>(_context);
        Specializations = new Repository<Specialization>(_context);
        Reviews = new Repository<Review>(_context);
        Clinics = new Repository<Clinic>(_context);
        Schedules = new Repository<DoctorSchedule>(_context);
        Nurses = new Repository<Nurse>(_context);
        Countries = new Repository<Country>(_context);
        Cities = new Repository<City>(_context);

    }
    public IRepository<User> Users { get; }
    public IRepository<Appointment> Appointments { get; }
    public IRepository<Billing> Billings { get; }
    public IRepository<Doctor> Doctors { get; }
    public IRepository<Review> Reviews { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Clinic> Clinics { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Specialization> Specializations { get; }
    public IRepository<Query> Queries { get; }
    public IRepository<DoctorSchedule>Schedules { get; }
    public IRepository<Nurse> Nurses { get; }
    public IRepository<Country> Countries { get; }
    public IRepository<City> Cities { get; }


    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
       return _context.SaveChanges();
    }
}
