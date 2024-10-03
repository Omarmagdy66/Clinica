using HospitalAPI.DTO;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HospitalAPI.Controllers
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorDto doctordto)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor()
                {
                    Name = doctordto.Name,
                    Email = doctordto.Email,
                    Password = doctordto.Password,
                    PhoneNumber = doctordto.PhoneNumber,
                    ClinicAddress = doctordto.ClinicAddress,
                    Specialty = doctordto.Specialty,
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
                doctor.Name = doctordto.Name;
                doctor.Email = doctordto.Email;
                doctor.Password = doctordto.Password;
                doctor.PhoneNumber = doctordto.PhoneNumber;
                doctor.ClinicAddress = doctordto.ClinicAddress;
                doctor.Specialty = doctordto.Specialty;
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
    }
}
