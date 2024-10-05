using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : APIBaseController
{
    public PatientController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        //var Patients = await _unitOfWork.Patients.GetAllAsync();
        return Ok(await _unitOfWork.Patients.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(patient);
    }
    [HttpPost]
    public async Task<IActionResult> AddPatient(PatientDto patientdto)
    {
        if (ModelState.IsValid)
        {
            var patient = new Patient()
            {
                Name = patientdto.Name,
                Email = patientdto.Email,
                Birthday = patientdto.Birthday,
                Gender = patientdto.Gender,
                Password = patientdto.Password,
                PhoneNumber = patientdto.PhoneNumber,
                RegistrationDate = patientdto.RegistrationDate
            };
            await _unitOfWork.Patients.AddAsync(patient);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditPatient(int id, [FromBody] PatientDto patientdto)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            patient.Name = patientdto.Name;
            patient.Email = patientdto.Email;
            patient.Birthday = patientdto.Birthday;
            patient.Gender = patientdto.Gender;
            patient.Password = patientdto.Password;
            patient.PhoneNumber = patientdto.PhoneNumber;
            patient.RegistrationDate = patientdto.RegistrationDate;
            _unitOfWork.Patients.Update(patient);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Patients.Delete(patient);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }
}
