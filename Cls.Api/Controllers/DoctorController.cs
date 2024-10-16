
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
using Cls.DAL.Migrations;
using BCrypt.Net;
using System.Linq.Expressions;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : APIBaseController
    {
        public DoctorController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

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


        [HttpPut("EditDoctor")]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> EditDoctor([FromBody] DoctorDto doctordto)
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

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            if (ModelState.IsValid)
            {
                var hashpass = BCrypt.Net.BCrypt.HashPassword(doctordto.Password);
                doctor.Name = doctordto.Name;
                doctor.Password = hashpass;
                doctor.PhoneNumber = doctordto.PhoneNumber;
                doctor.SpecializationId = doctordto.SpecializationId;
                doctor.Bio = doctordto.Bio;
                doctor.Price = doctordto.Price;
                doctor.Image = doctordto.Image;
                doctor.Examinationduration = doctordto.Examinationduration;
                _unitOfWork.Doctors.Update(doctor);
                var user = await _unitOfWork.Users.FindAsync(n => n.Email == doctor.Email);
                user.Password = hashpass;
                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
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


        [HttpPost("RequestClinic")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> RequestClinic(int clinicid, ClinicDto? clinicDto)
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


            var clinic = await _unitOfWork.Clinics.GetByIdAsync(clinicid);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
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

                return Ok("Wating");
            }
            else
            {
                return Ok("please fill clinic data");
            }

        }


        //[HttpPut("UpdateRequestClinic")]
        //[Authorize(Roles = "3")]
        //public async Task<IActionResult> UpdateRequestClinic(int oldclinicid, int newclinicid, ClinicDto clinicDto)
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
        //    var clinic = await _unitOfWork.Clinics.GetByIdAsync(newclinicid);
        //    var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
        //    if (doctor == null)
        //    {
        //        return BadRequest("Invalid doctor Id ");

        //    }
        //        var adminrequest = new adminRequest()
        //        {
        //            ClinicName = clinicDto.ClinicName,
        //            Address = clinicDto.Address,
        //            PhoneNumber = clinicDto.PhoneNumber,
        //            CityId = clinicDto.CityId,
        //            CountryId = clinicDto.CountryId,
        //            DoctorId = doctorId
        //        };
        //        await _unitOfWork.AdminRequests.AddAsync(adminrequest);
        //        _unitOfWork.Save();

        //        return Ok("Wating");
        //}


        [HttpGet("Getnotifications")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetNotifications()
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
            var notifications = await _unitOfWork.Notifications.FindAllAsync(d => d.Doctorid == doctorId);

            return Ok(notifications);
        }


        [HttpDelete("ExistFromClinic")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> ExistFromClinic(int clinicid)
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
            var result = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorId, clinicid);
            _unitOfWork.DoctorClinics.Delete(result);
            _unitOfWork.Save();
            return Ok("You Exist Successfully");
        }

        [HttpGet("DoctorSearch")]
        public async Task<IActionResult> DoctorSearch(int? Specialization, int? Country, int? city)
        {
            if ((Specialization == null && Country == null && city == null) || (Specialization == null && Country == null && city != null))
            {
                var Doctors = await _unitOfWork.Doctors.GetAllAsync();
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
            else if (Country != null && Specialization != null && city != null)
            {
                var Doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.DoctorCLinics.Any(dc => dc.Clinic.CountryId == Country && dc.Clinic.CityId == city && d.SpecializationId == Specialization));
                if (Doctors == null || !Doctors.Any())
                    return BadRequest("There are no doctors");
                return Ok(Doctors);
            }
            return BadRequest();

        }

    }
}