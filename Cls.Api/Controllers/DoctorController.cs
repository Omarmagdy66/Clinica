
using Interfaces;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dto;
using System.Numerics;
using Controllers;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : APIBaseController
    {
        public DoctorController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            //var doctors = await _unitOfWork.Doctors.GetAllAsync();
            return Ok(await _unitOfWork.Doctors.GetAllAsync());
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
//                 Examinationduration = doctordto.Examinationduration,
//                 Name = doctordto.Name,
//                 Bio = doctordto.Bio,
//                 Email = doctordto.Email,
//                 Image = doctordto.Image,
//                 Password = doctordto.Password,
//                 PhoneNumber = doctordto.PhoneNumber,
//                 Price = doctordto.Price,
//                 SpecializationId = doctordto.SpecializationId,
//                 DoctorCLinics = new List<DoctorCLinic>()
//             };

            
//             var doctorClinic = new DoctorCLinic()
//             {
//                 ClinicId = doctordto.ClinicId,
//                 Doctor = doctor 
//             };

//             doctor.DoctorCLinics.Add(doctorClinic);
//             await _unitOfWork.Doctors.AddAsync(doctor);
//             _unitOfWork.Save();
//             return Ok("Created!");
                return BadRequest("Invalid Id");
            }
            return Ok(doctor);
        }
        [HttpGet("Getnotifications")]
        public async Task<IActionResult> GetNotifications(int id)
        {
            var notifications = await _unitOfWork.Notifications.FindAllAsync(d=>d.Doctorid==id);
          
            return Ok(notifications);
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorDto doctordto)
        {
            if (ModelState.IsValid)
            {
                var specialty = await _unitOfWork.Specializations.GetByIdAsync(doctordto.SpecializationId);
                if (specialty == null)
                {
                    return NotFound("Specialty not found.");
                }
                var doctor = new Doctor()
                {
                    Name = doctordto.Name,
                    Email = doctordto.Email,
                    Password = doctordto.Password,
                    PhoneNumber = doctordto.PhoneNumber,
 
                     SpecializationId = doctordto.SpecializationId,
                     Price = doctordto.Price,
                     Bio=doctordto.Bio, 
                     Image=doctordto.Image, 
                    DoctorCLinics = new List<DoctorCLinic>()

                };

              

                       
                    
                
                await _unitOfWork.Doctors.AddAsync(doctor);
                _unitOfWork.Save();
                return Ok("Created!");
            }
            return BadRequest($"ther are {ModelState.ErrorCount} errors");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDoctor(int id, [FromBody] DoctorDto doctordto)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            if (ModelState.IsValid)
            {
                var specialty = await _unitOfWork.Specializations.GetByIdAsync(doctordto.SpecializationId);
                if (specialty == null)
                {
                  
                    return NotFound("Specialty not found.");
                }
                doctor.Name = doctordto.Name;
                doctor.Email = doctordto.Email;
                doctor.Password = doctordto.Password;
                doctor.PhoneNumber = doctordto.PhoneNumber;
                doctor.SpecializationId = doctordto.SpecializationId;
                doctor.Bio=doctordto.Bio;   
                doctor.Price=doctordto.Price;   
                doctor.Image=doctordto.Image;   
               

                _unitOfWork.Save();
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            _unitOfWork.Doctors.Delete(doctor);
            _unitOfWork.Save();
            return Ok("Deleted!");
        }
        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic(int clinicid,int doctorid,ClinicDto clinicDto )
        {
            var clinic=await _unitOfWork.Clinics.GetByIdAsync(clinicid);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorid); 
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            if (clinic == null) {
                var adminrequest = new adminRequest()
                {
                    ClinicName=clinicDto.ClinicName,
                    Address=clinicDto.Address,
                    PhoneNumber=clinicDto.PhoneNumber,
                    CityId=clinicDto.CityId,
                    CountryId=clinicDto.CountryId,
                    DoctorId=doctorid,
                   

                };
                await _unitOfWork.AdminRequests.AddAsync(adminrequest);
                _unitOfWork.Save();

                return Ok("Wating");
            }

            var doctorclinic = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorid, clinicid);
            if (doctorclinic != null)
            {
                return Ok("the Clinic is already added");
                
            } 
                doctorclinic = new DoctorCLinic();
                doctorclinic.DoctorId = doctorid;
                doctorclinic.ClinicId = clinicid;
            await _unitOfWork.DoctorClinics.AddAsync(doctorclinic);
                _unitOfWork.Save();

                return Ok("added");

        }
        [HttpPut("UpdateClinic")]
        public async Task<IActionResult> UpdateClinic(int oldclinicid,int newclinicid, int doctorid, ClinicDto clinicDto)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(newclinicid);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorid); 
            if (doctor == null)
            {
                return BadRequest("Invalid doctor Id ");

            }
            if (clinic == null)
            {
                var adminrequest = new adminRequest()
                {
                    ClinicName = clinicDto.ClinicName,
                    Address = clinicDto.Address,
                    PhoneNumber = clinicDto.PhoneNumber,
                    CityId = clinicDto.CityId,
                    CountryId = clinicDto.CountryId,
                    DoctorId = doctorid,


                };
                await _unitOfWork.AdminRequests.AddAsync(adminrequest);
                _unitOfWork.Save();

                return Ok("Wating");
            }
           
            var doctorclinic = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorid, newclinicid);
            if (doctorclinic != null)
            {
                return Ok("the Clinic is already Exits");

            }
             doctorclinic = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorid, oldclinicid);
            _unitOfWork.DoctorClinics.Delete(doctorclinic);
            doctorclinic = new DoctorCLinic();
            doctorclinic.DoctorId = doctorid;
            doctorclinic.ClinicId = newclinicid;
            await _unitOfWork.DoctorClinics.AddAsync(doctorclinic);
            _unitOfWork.Save();
      

            return Ok("Updated");
        }
        [HttpDelete("DeleteClinic")]
        public async Task<IActionResult>DeleteClinic(int clinicid,int doctorid)
        {
            var result = await _unitOfWork.DoctorClinics.GetByCompositeAsync(doctorid, clinicid);
            _unitOfWork.DoctorClinics.Delete(result);
            _unitOfWork.Save();
            return Ok("Deleted");
        }



    }
}

