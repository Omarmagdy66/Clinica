using Data;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        protected clinicdbContext _context;
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
            Admins = new Repository<Admin>(_context);
            AdminRequests = new Repository<adminRequest>(_context);
            NurseAdminRequests = new Repository<NurseAdminRequest>(_context);
            DoctorClinics = new Repository<DoctorCLinic>(_context);
            Notifications = new Repository<Notification>(_context);
            NurseNotifications = new Repository<NurseNotification>(_context);
            ApplyDoctorRequests = new Repository<ApplyDoctorRequest>(_context);

        }
        public IRepository<User> Users { get; }
        public IRepository<Appointment> Appointments { get; }
        public IRepository<Billing> Billings { get; }
        public IRepository<Doctor> Doctors { get; }
        public IRepository<Review> Reviews { get; }
        public IRepository<DoctorCLinic> DoctorClinics { get; }
        public IRepository<Role> Roles { get; }
        public IRepository<Clinic> Clinics { get; }
        public IRepository<Patient> Patients { get; }
        public IRepository<Specialization> Specializations { get; }
        public IRepository<Query> Queries { get; }
        public IRepository<DoctorSchedule> Schedules { get; }
        public IRepository<Nurse> Nurses { get; }
        public IRepository<Country> Countries { get; }
        public IRepository<City> Cities { get; }
        public IRepository<Admin> Admins { get; }
        public IRepository<adminRequest> AdminRequests { get; }
        public IRepository<Notification> Notifications { get; }
        public IRepository<NurseNotification> NurseNotifications { get; }
        public IRepository<NurseAdminRequest> NurseAdminRequests { get; }
        public IRepository<ApplyDoctorRequest> ApplyDoctorRequests { get; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
