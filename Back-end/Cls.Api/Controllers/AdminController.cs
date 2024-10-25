using Interfaces;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dto;
using System.Numerics;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : APIBaseController
    {
        private readonly IEmailService _emailService;
        public AdminController(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
        {
            _emailService = emailService;
        }

        [HttpGet("GetAllRequestsClinic")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllRequestsClinic()
        {
            return Ok(await _unitOfWork.AdminRequests.GetAllAsync());
        }

        [HttpGet("GetAllRequestsApply")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllRequestsApply()
        {
            return Ok(await _unitOfWork.ApplyDoctorRequests.GetAllAsync());
        }



        [HttpPost("AdminResponseDoctor")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AdminResponseDoctor(int id, int flag)
        {
            var request = await _unitOfWork.AdminRequests.GetByIdAsync(id);
            var clinicname = await _unitOfWork.Clinics.FindAsync(n => n.ClinicName == request.ClinicName);
            if (flag == 1)
            {
                if (clinicname == null)
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
                    var notification = new Notification();
                    notification.Doctorid = request.DoctorId;
                    notification.message = "The Clinic (\"" + request.ClinicName + "\") Whose Addrease Is " + request.Address + " Added Succefully ";
                    await _unitOfWork.Notifications.AddAsync(notification);
                    var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
                    var Email = new EmailDto()
                    {
                        Subject = $"Clinic Join Request Approved: Dr. {doctor.Name}",
                        Body = $"Dear Dr. {doctor.Name},<br><br>We are pleased to inform you that your request to join the clinic (\" {clinic.ClinicName} \") has been approved. You are now officially part of the clinic, and you can begin managing your appointments and add your schedule other activities through our system.<br><br>Should you have any questions or need further assistance, please feel free to reach out to us.<br><br>Thank you for choosing CLINICA. We look forward to supporting your success.<br><br>Best regards,<br>CLINICA",
                        To = doctor.Email
                    };
                    _emailService.SendEmail(Email);

                    _unitOfWork.AdminRequests.Delete(request);
                    _unitOfWork.Save();

                    return Ok("responsed");
                }
                else
                {
                    var doctorclinic = new DoctorCLinic();
                    doctorclinic.DoctorId = request.DoctorId;
                    doctorclinic.ClinicId = clinicname.Id;
                    await _unitOfWork.DoctorClinics.AddAsync(doctorclinic);
                    var notification = new Notification();
                    notification.Doctorid = request.DoctorId;
                    notification.message = "The Clinic (\""+ request.ClinicName +"\") Whose Addrease Is " + request.Address + " Added Succefully ";
                    await _unitOfWork.Notifications.AddAsync(notification);
                    var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
                    var Email = new EmailDto()
                    {
                        Subject = $"Clinic Join Request Approved: Dr. {doctor.Name}",
                        Body = $"Dear Dr. {doctor.Name},<br><br>We are pleased to inform you that your request to join the clinic (\" {clinicname.ClinicName} \") has been approved. You are now officially part of the clinic, and you can begin managing your appointments and add your schedule other activities through our system.<br><br>Should you have any questions or need further assistance, please feel free to reach out to us.<br><br>Thank you for choosing CLINICA. We look forward to supporting your success.<br><br>Best regards,<br>CLINICA",
                        To = doctor.Email
                    };
                    _emailService.SendEmail(Email);


                    _unitOfWork.AdminRequests.Delete(request);
                    _unitOfWork.Save();

                    return Ok("responsed");
                }
            }

            else
            {
                var notification = new Notification();
                notification.Doctorid = request.DoctorId;
                notification.message = "The Clinic Whose Addrease Is " + request.Address + " Doesnt Exits  ";
                await _unitOfWork.Notifications.AddAsync(notification);
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
                var Email = new EmailDto()
                {
                    Subject = $"Clinic Join Request Declined: Dr. {doctor.Name}",
                    Body = $"Dear Dr. {doctor.Name},<br><br>We regret to inform you that your request to join the clinic has been declined. After reviewing your request, the clinic has decided not to proceed at this time.<br><br>If you have any questions or require further clarification, please feel free to contact us. We appreciate your interest in Clinica and encourage you to explore other opportunities within our network.<br><br>Thank you for your understanding.<br><br>Best regards,<br>CLINICA",
                    To = doctor.Email
                };
                _emailService.SendEmail(Email);

                _unitOfWork.AdminRequests.Delete(request);
                _unitOfWork.Save();
                return Ok("responsed");
            }
        }

        [HttpPost("AdminResponseApplyDoctor")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AdminResponseApplyDoctor(int id, int flag)
        {
            var applyrequest = await _unitOfWork.ApplyDoctorRequests.GetByIdAsync(id);
            if (flag == 1)
            {
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(applyrequest.DoctorId);
                if (doctor != null)
                {

                    doctor.Bio = applyrequest.Bio;
                    doctor.Examinationduration = applyrequest.Examinationduration;
                    doctor.SpecializationId = applyrequest.SpecializationId;
                    doctor.Price = applyrequest.Price;

                    _unitOfWork.Doctors.Update(doctor);

                    var notification = new Notification();
                    notification.Doctorid = applyrequest.DoctorId;
                    notification.message = "Approved For Doctor\n Go to Join Clinic";
                    await _unitOfWork.Notifications.AddAsync(notification);

                    var Email = new EmailDto()
                    {
                        Subject = $"Application Approved: Dr. {doctor.Name}",
                        Body = $"Dear Dr. {doctor.Name},<br><br>We are pleased to inform you that your application to join Clinica has been approved. You are now officially part of our network, and you can begin managing your profile, appointments, and other services through our platform.<br><br>If you have any questions or need assistance getting started, please do not hesitate to contact us.<br><br>Welcome to Clinica, and we look forward to working with you.<br><br>Best regards,<br>CLINICA",
                        To = doctor.Email
                    };
                    _emailService.SendEmail(Email);

                    _unitOfWork.ApplyDoctorRequests.Delete(applyrequest);
                    _unitOfWork.Save();

                    return Ok("responsed");
                }
                else
                {
                    return NotFound("Not Found");
                }
            }
            else
            {
                var notification = new Notification();
                notification.Doctorid = applyrequest.DoctorId;
                notification.message = "Approved For Doctor\n Go to Join Clinic";
                await _unitOfWork.Notifications.AddAsync(notification);
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(applyrequest.DoctorId);
                var Email = new EmailDto()
                {
                    Subject = $"Application Declined: Dr. {doctor.Name}",
                    Body = $"Dear Dr. {doctor.Name},<br><br>We regret to inform you that your application to join Clinica has been declined. After reviewing your application, we have decided not to proceed with your request at this time.<br><br>If you have any questions or would like further clarification, please feel free to reach out to us. We appreciate your interest in Clinica and encourage you to explore future opportunities within our network.<br><br>Thank you for your understanding.<br><br>Best regards,<br>CLINICA",
                    To = doctor.Email
                };
                _emailService.SendEmail(Email);
                return Ok("responsed");
            }
        }

        [HttpPost("AdminResponseNurse")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AdminResponseNurse(int id, int flag)
        {
            var request = await _unitOfWork.NurseAdminRequests.GetByIdAsync(id);
            var nurse = await _unitOfWork.Nurses.GetByIdAsync(request.NurseId);
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
                nurse.ClinicId = clinicId;
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
