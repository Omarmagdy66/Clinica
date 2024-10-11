using Interfaces;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : APIBaseController
    {
        public AdminController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpPost("AdminResponseDoctor")]
        public async Task<IActionResult> AdminResponseDoctor(int id, int flag)
        {
            var request = await _unitOfWork.AdminRequests.GetByIdAsync(id);
            if (flag == 1)
            {
                var clinic = new Clinic()
                {
                    PhoneNumber = request.PhoneNumber,
                    ClinicName = request.ClinicName,
                    CountryId = request.CountryId,
                    CityId = request.CityId,
                    Address = request.Address,
                };
                await _unitOfWork.Clinics.AddAsync(clinic);
                _unitOfWork.Save();
                var results = await _unitOfWork.Clinics.GetAllAsync();
                int clinicId = 0;
                foreach (var result in results)
                {
                    clinicId = result.Id;
                }
                var doctorclinic = new DoctorCLinic();
                doctorclinic.DoctorId = request.DoctorId;
                doctorclinic.ClinicId = clinicId;
                await _unitOfWork.DoctorClinics.AddAsync(doctorclinic);
                var notification= new Notification();
                notification.Doctorid =request.DoctorId;
                notification.message = "the clinic whose addrease is " + request.Address + "added succefully ";
                await _unitOfWork.Notifications.AddAsync(notification);

                _unitOfWork.AdminRequests.Delete(request);
                _unitOfWork.Save();

                return Ok("responsed");
            }

            else
            {
                var notification = new Notification();
                notification.Doctorid = request.DoctorId;
                notification.message = "the clinic whose addrease is " + request.Address + "doesnt exits  ";
                await _unitOfWork.Notifications.AddAsync(notification);
                _unitOfWork.AdminRequests.Delete(request);
                return Ok("responsed");
            }
        }
        [HttpPost("AdminResponseNurse")]
        public async Task<IActionResult> AdminResponseNurse(int id, int flag)
        {
            var request = await _unitOfWork.NurseAdminRequests.GetByIdAsync(id);
            var nurse=await _unitOfWork.Nurses.GetByIdAsync(request.NurseId);
            if (flag == 1)
            {
                var clinic = new Clinic()
                {
                    PhoneNumber = request.PhoneNumber,
                    ClinicName = request.ClinicName,
                    CountryId = request.CountryId,
                    CityId = request.CityId,
                    Address = request.Address,
                };
                await _unitOfWork.Clinics.AddAsync(clinic);
                _unitOfWork.Save();
                var results = await _unitOfWork.Clinics.GetAllAsync();
                int clinicId = 0;
                foreach (var result in results)
                {
                    clinicId = result.Id;
                }
                nurse.ClinicId= clinicId;
                var notification = new NurseNotification();
                notification.Nurseid = request.NurseId;
                notification.message = "the clinic whose addrease is " + request.Address + "added  ";
                await _unitOfWork.NurseNotifications.AddAsync(notification);
             
                _unitOfWork.NurseAdminRequests.Delete(request);
                _unitOfWork.Save();

                return Ok("responsed");
            }

            else
            {
                var notification = new NurseNotification();
                notification.Nurseid = request.NurseId;
                notification.message = "the clinic whose addrease is " + request.Address + "doesnt exits  ";
                await _unitOfWork.NurseNotifications.AddAsync(notification);
                _unitOfWork.NurseAdminRequests.Delete(request);
                _unitOfWork.Save();
                return Ok("responsed");
            }
        }
    }
}
