using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

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
                Examinationduration = doctordto.Examinationduration,
                Name = doctordto.Name,
                Bio = doctordto.Bio,
                Email = doctordto.Email,
                Image = doctordto.Image,
                Password = doctordto.Password,
                PhoneNumber = doctordto.PhoneNumber,
                Price = doctordto.Price,
                SpecializationId = doctordto.SpecializationId,
                DoctorCLinics = new List<DoctorCLinic>()
            };

            
            var doctorClinic = new DoctorCLinic()
            {
                ClinicId = doctordto.ClinicId,
                Doctor = doctor 
            };

            doctor.DoctorCLinics.Add(doctorClinic);
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
            doctor.Examinationduration = doctordto.Examinationduration;
            doctor.Name = doctordto.Name;
            doctor.Bio = doctordto.Bio;
            doctor.Email = doctordto.Email;
            doctor.Image = doctordto.Image;
            doctor.Password = doctordto.Password;
            doctor.PhoneNumber = doctordto.PhoneNumber;
            doctor.Price = doctordto.Price;
            doctor.SpecializationId = doctordto.SpecializationId;
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
