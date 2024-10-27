
using Interfaces;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dto;
using System.Numerics;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using BCrypt.Net;
using System.Linq.Expressions;
using Services;
using Cls.Api.Dto;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : APIBaseController
    {
        private readonly IEmailService _emailService;


        public DoctorController(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
        {
            _emailService = emailService;
        }


        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            return Ok(await _unitOfWork.Doctors.GetAllAsync());
        }


        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(doctor);
        }


        [HttpPost("AddDoctor")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddDoctor(DoctorDto doctordto)
        {
            if (ModelState.IsValid)
            {
                var hashpass = BCrypt.Net.BCrypt.HashPassword(doctordto.Password);
                var user = new User()
                {
                    UserName = doctordto.Name,
                    Email = doctordto.Email,
                    Password = hashpass,
                };
                var doctor = new Doctor()
                {
                    Name = doctordto.Name,
                    Email = doctordto.Email,
                    Password = hashpass,
                    PhoneNumber = doctordto.PhoneNumber,
                    Examinationduration = doctordto.Examinationduration,
                    SpecializationId = doctordto.SpecializationId,
                    Price = doctordto.Price,
                    Bio = doctordto.Bio,
                    Image = doctordto.Image,
                };
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.Doctors.AddAsync(doctor);
                _unitOfWork.Save();
                return Ok("Created!");
            }
            return BadRequest($"ther are {ModelState.ErrorCount} errors");
        }


        //[HttpPut("EditDoctor")]
        //[Authorize(Roles = "1,3")]
        //public async Task<IActionResult> EditDoctor([FromBody] DoctorDto doctordto)
        //{
        //    var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    if (doctorIdClaim == null)
        //    {
        //        return Unauthorized("Doctor ID not found in token.");
        //    }

        //    // Parse the DoctorId
        //    int doctorId;
        //    if (!int.TryParse(doctorIdClaim, out doctorId))
        //    {
        //        return BadRequest("Invalid Doctor ID in token.");
        //    }

        //    var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
        //    if (doctor == null)
        //    {
        //        return BadRequest("Invalid Id");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var hashpass = BCrypt.Net.BCrypt.HashPassword(doctordto.Password);
        //        doctor.Name = doctordto.Name;
        //        doctor.Password = hashpass;
        //        doctor.PhoneNumber = doctordto.PhoneNumber;
        //        doctor.SpecializationId = doctordto.SpecializationId;
        //        doctor.Bio = doctordto.Bio;
        //        doctor.Price = doctordto.Price;
        //        doctor.Image = doctordto.Image;
        //        doctor.Examinationduration = doctordto.Examinationduration;
        //        _unitOfWork.Doctors.Update(doctor);
        //        var user = await _unitOfWork.Users.FindAsync(n => n.Email == doctor.Email);
        //        user.Password = hashpass;
        //        _unitOfWork.Users.Update(user);
        //        _unitOfWork.Save();
        //        return Ok("Updated!");
        //    }
        //    return BadRequest($"There are {ModelState.ErrorCount}");
        //}


        [HttpPut("EditDoctor")]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> EditDoctor([FromBody] DoctorDto doctordto)
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            var user = await _unitOfWork.Users.FindAsync(n => n.Email == doctor.Email);
            if (ModelState.IsValid)
            {
                // Only update the fields that have been provided (not null or empty)
                if (!string.IsNullOrEmpty(doctordto.Name) && doctor.Name != doctordto.Name)
                {
                    var user1 = await _unitOfWork.Users.FindAsync(u => u.UserName == doctordto.Name);
                    if (user1 != null)
                        return BadRequest("Name already Exist!");
                    doctor.Name = doctordto.Name;
                    user.UserName = doctordto.Name;
                }

                if (!string.IsNullOrEmpty(doctordto.Password) && BCrypt.Net.BCrypt.Verify(doctordto.Password, doctor.Password))
                {
                    var hashpass = BCrypt.Net.BCrypt.HashPassword(doctordto.Password);
                    doctor.Password = hashpass;
                    user.Password = hashpass;

                }

                if (!string.IsNullOrEmpty(doctordto.Email) && doctor.Email != doctordto.Email)
                {
                    var user1 = await _unitOfWork.Users.FindAsync(u => u.Email == doctordto.Email);
                    if (user1 != null)
                        return BadRequest("Email already Exist!");
                    doctor.Email = doctordto.Email;
                    user.Email = doctordto.Email;
                }

                if (!string.IsNullOrEmpty(doctordto.PhoneNumber) && doctor.PhoneNumber != doctordto.PhoneNumber)
                {
                    doctor.PhoneNumber = doctordto.PhoneNumber;
                }

                if (doctordto.SpecializationId.HasValue && doctor.SpecializationId != doctordto.SpecializationId)
                {
                    doctor.SpecializationId = doctordto.SpecializationId.Value;
                }

                if (!string.IsNullOrEmpty(doctordto.Bio) && doctor.Bio != doctordto.Bio)
                {
                    doctor.Bio = doctordto.Bio;
                }

                if (doctordto.Price.HasValue && doctor.Price != doctordto.Price)
                {
                    doctor.Price = doctordto.Price.Value;
                }

                if (!string.IsNullOrEmpty(doctordto.Image) && doctor.Image != doctordto.Image)
                {
                    doctor.Image = doctordto.Image;
                }

                if (doctordto.Examinationduration.HasValue && doctor.Examinationduration != doctordto.Examinationduration)
                {
                    doctor.Examinationduration = doctordto.Examinationduration.Value;
                }

                // Update the doctor
                _unitOfWork.Doctors.Update(doctor);
                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();

                return Ok("Updated!");
            }

            return BadRequest($"There are {ModelState.ErrorCount} errors in the model.");
        }



        [HttpDelete("DeleteDoctor")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            _unitOfWork.Doctors.Delete(doctor);
            var doctorclinic = await _unitOfWork.DoctorClinics.FindAllAsync(n => n.DoctorId == doctor.Id);
            _unitOfWork.DoctorClinics.DeleteRange(doctorclinic);
            _unitOfWork.Save();
            return Ok("Deleted!");
        }

        //check if is apply a Doctor
        [HttpPost("RequestClinic")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> RequestClinic(int clinicid, ClinicDto? clinicDto)
        {

            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            if (!int.TryParse(doctorIdClaim, out int doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }


            var clinic = await _unitOfWork.Clinics.GetByIdAsync(clinicid);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            if (doctor.Bio != null && doctor.SpecializationId != null && doctor.Examinationduration != null && doctor.Price != null)
            {
                var doctorclinic = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorId, clinicid);
                {
                    if (doctorclinic != null)
                        return Ok("the Clinic is already Exits");
                }
                if (clinic != null)
                {
                    var adminrequest = new adminRequest()
                    {
                        ClinicName = clinic.ClinicName,
                        Address = clinic.Address,
                        PhoneNumber = clinic.PhoneNumber,
                        CityId = clinic.CityId,
                        CountryId = clinic.CountryId,
                        DoctorId = doctorId
                    };
                    await _unitOfWork.AdminRequests.AddAsync(adminrequest);
                    _unitOfWork.Save();
                    var Email = new EmailDto()
                    {
                        Subject = $"Clinic Join Request Received: Dr. {doctor.Name}",
                        Body = $"Dear Dr. {doctor.Name},<br><br>We have received your request to join the clinic (\"{clinic.ClinicName}\"), and our team is currently processing it. We will review the details and notify you once the clinic has approved your request.<br><br>If any further information is needed, we will reach out to you directly.<br><br>Thank you for choosing Clinica, and we look forward to facilitating your collaboration with the clinic.<br><br>Best regards,<br>CLINICA",
                        To = doctor.Email
                    };
                    _emailService.SendEmail(Email);
                    return Ok("Wating");
                }
                else if (clinicDto != null)
                {
                    var adminrequest = new adminRequest()
                    {
                        ClinicName = clinicDto.ClinicName,
                        Address = clinicDto.Address,
                        PhoneNumber = clinicDto.PhoneNumber,
                        CityId = clinicDto.CityId,
                        CountryId = clinicDto.CountryId,
                        DoctorId = doctorId
                    };
                    await _unitOfWork.AdminRequests.AddAsync(adminrequest);
                    _unitOfWork.Save();

                    var Email = new EmailDto()
                    {
                        Subject = $"Clinic Addition Request: Dr. {doctor.Name}",

                        Body = $"Dear Dr. {doctor.Name},<br><br>We have received your request to add a new clinic (\"{clinicDto.ClinicName}\") to our system. Our team will review the details and process your request as soon as possible.<br><br>If we need further information, we will contact you directly.<br><br>Thank you for your continued trust in Clinica.<br><br>Best regards,<br>CLINICA",
                        To = doctor.Email
                    };
                    _emailService.SendEmail(Email);

                    return Ok("Wating");
                }
                else
                {
                    return Ok("please fill clinic data");
                }
            }
            else
            {
                return BadRequest("Must apply a Doctor Frist!");
            }

        }


        [HttpPost("RequestApplyDoctor")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> RequestApplyDoctor(ApplyDoctorDto applydoctordto)
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }


            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            var ApplydoctorCount = _unitOfWork.ApplyDoctorRequests.Count(o => o.DoctorId == doctorId);
            if (ApplydoctorCount < 1)
            {
                var Applydoctor = new ApplyDoctorRequest()
                {
                    DoctorId = doctorId,
                    Bio = applydoctordto.Bio,
                    Price = applydoctordto.Price,
                    Examinationduration = applydoctordto.Examinationduration,
                    SpecializationId = applydoctordto.SpecializationId,
                    Email = doctor.Email,
                    PhoneNumber = doctor.PhoneNumber,
                    Name = doctor.Name
                };
                await _unitOfWork.ApplyDoctorRequests.AddAsync(Applydoctor);
                _unitOfWork.Save();
                var Email = new EmailDto()
                {
                    Subject = $"Doctor Application Received: Dr. {doctor.Name}",
                    Body = $"Dear Dr. {doctor.Name},<br><br>Thank you for applying to join Clinica. We have received your application, and our team is currently reviewing it.<br><br>We will contact you shortly to provide further details or request additional information if necessary.<br><br>We appreciate your interest in becoming part of Clinica and look forward to the possibility of working together.<br><br>Best regards,<br>CLINICA",
                    To = doctor.Email
                };
                _emailService.SendEmail(Email);
                return Ok("Request Submitted\nWating");
            }
            else
            {
                return BadRequest("you Applied Before");
            }
        }


        [HttpPut("UpdateRequestClinic")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> UpdateRequestClinic(int oldclinicid, int newclinicid, ClinicDto clinicDto)
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(newclinicid);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            var adminrequest = new adminRequest()
            {
                ClinicName = clinicDto.ClinicName,
                Address = clinicDto.Address,
                PhoneNumber = clinicDto.PhoneNumber,
                CityId = clinicDto.CityId,
                CountryId = clinicDto.CountryId,
                DoctorId = doctorId
            };
            await _unitOfWork.AdminRequests.AddAsync(adminrequest);
            _unitOfWork.Save();

            return Ok("Wating");
        }


        [HttpGet("Getnotifications")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetNotifications()
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }
            var notifications = await _unitOfWork.Notifications.FindAllAsync(d => d.Doctorid == doctorId);

            return Ok(notifications);
        }


        [HttpDelete("ExitFromClinic")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> ExitFromClinic(int clinicid)
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }
            var Schedule = await _unitOfWork.Schedules.FindAllAsync(s => s.DoctorId == doctorId && s.ClinicId == clinicid);
            _unitOfWork.Schedules.DeleteRange(Schedule);
            var result = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorId, clinicid);
            _unitOfWork.DoctorClinics.Delete(result);
            var IsEmpty = await _unitOfWork.Doctors.FindAllAsync(c => c.DoctorCLinics.Any(dc => dc.ClinicId == clinicid));
            if (IsEmpty != null)
            {
                _unitOfWork.Save();
                return Ok("You Exist Successfully");
            }
            else
            {
                var clinic = await _unitOfWork.Clinics.GetByIdAsync(clinicid);
                _unitOfWork.Clinics.Delete(clinic);
                _unitOfWork.Save();
                return Ok("You Exist Successfully");
            }
        }

        [HttpGet("DoctorSearch")]
        public async Task<IActionResult> DoctorSearch(int? Specialization, int? Country, int? city)
        {
            if ((Specialization == null && Country == null && city == null) || (Specialization == null && Country == null && city != null))
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.Bio != null && d.SpecializationId != null && d.Examinationduration != null && d.Price != null);
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors");

                return Ok(Doctors);
            }
            else if (Specialization != null && Country == null && city == null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(n => n.SpecializationId == Specialization);
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors in this Specialization");
                return Ok(Doctors);

            }
            else if (Country != null && Specialization == null && city == null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.DoctorCLinics.Any(dc => dc.Clinic.CountryId == Country));
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors in this Country");
                return Ok(Doctors);
            }
            else if (Country != null && Specialization == null && city != null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.DoctorCLinics.Any(dc => dc.Clinic.CountryId == Country && dc.Clinic.CityId == city));
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors in this City");
                return Ok(Doctors);
            }
            else if (Country != null && Specialization != null && city == null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d =>d.SpecializationId == Specialization && d.DoctorCLinics.Any(dc => dc.Clinic.CountryId == Country));
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors in this City");
                return Ok(Doctors);
            }
            else if (Country != null && Specialization != null && city != null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.DoctorCLinics.Any(dc => dc.Clinic.CountryId == Country && dc.Clinic.CityId == city && d.SpecializationId == Specialization));
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors");
                return Ok(Doctors);
            }
            return BadRequest();

        }

        [HttpGet("IsDoctor")]
        public async Task<IActionResult> IsDoctor()
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            // Parse the DoctorId
            int doctorId;
            if (!int.TryParse(doctorIdClaim, out doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            if (doctor.Bio != null && doctor.SpecializationId != null && doctor.Examinationduration != null && doctor.Price != null)
            {
                return Ok("is Doctor");
            }
            else
            {
                return BadRequest("Not a doctor");
            }
        }

    }
}