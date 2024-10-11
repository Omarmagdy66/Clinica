using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClinicController : APIBaseController
{
    public ClinicController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAllClinics()
    {
        //var Clinics = await _unitOfWork.Clinics.GetAllAsync();
        return Ok(await _unitOfWork.Clinics.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClinicById(int id)
    {
        var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
        if (clinic == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(clinic);
    }
    [HttpGet("GetClinicByCityId")]
    public async Task<IActionResult> GetClinicByCityId(int CityId)
    {
        var clinics = await _unitOfWork.Clinics.GetAllAsync();
        var clinic = clinics.FirstOrDefault(x => x.CityId == CityId);
        if (clinic == null)
        {
            return BadRequest($"there isn't clinic in the city with id {CityId}");
        }
        return Ok(clinic);
    }
    [HttpGet("GetClinicByCountryId")]
    public async Task<IActionResult> GetClinicByCountryId(int CountryId)
    {
        var clinics = await _unitOfWork.Clinics.GetAllAsync();
        var clinic = clinics.FirstOrDefault(x => x.CountryId == CountryId);
        if (clinic == null)
        {
            return BadRequest($"there isn't clinic in the city with id {CountryId}");
        }
        return Ok(clinic);
    }
    [HttpGet("GetClinicBySpecialization")]
    public async Task<IActionResult> GetClinicBySpecialization(int specializationId)
    {
        var clinics = await _unitOfWork.Clinics.FindAllAsync(clinic => clinic.DoctorCLinics.Any(dc => dc.Doctor.SpecializationId == specializationId));

        if (!clinics.Any())
        {
            return NotFound("No clinics found offering this specialization.");
        }
        return Ok(clinics);
    }
    [HttpGet("GetClinicByDoctor")]
    public async Task<IActionResult> GetClinicByDoctor(int doctorId)
    {
        var clinics = await _unitOfWork.Clinics.FindAllAsync(clinic => clinic.DoctorCLinics.Any(doctor => doctor.DoctorId == doctorId));

        if (!clinics.Any())
        {
            return NotFound("No clinics found that have the provided doctor");
        }
        return Ok(clinics);
    }
    [HttpPost]
    public async Task<IActionResult> AddLocation(ClinicDto clinicDto)
    {
        if (ModelState.IsValid)
        {
            var clinic = new Clinic()
            {
                Image = clinicDto.Image,
                Address = clinicDto.Address,
                ClinicName = clinicDto.ClinicName,
                CountryId = clinicDto.CountryId,
                PhoneNumber = clinicDto.PhoneNumber,
                CityId = clinicDto.CityId,
            };
            await _unitOfWork.Clinics.AddAsync(clinic);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditLocation(int id, [FromBody] ClinicDto clinicDto)
    {
        var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
        if (clinic == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            clinic.Image = clinicDto.Image;
            clinic.Address = clinicDto.Address;
            clinic.ClinicName = clinicDto.ClinicName;
            clinic.CountryId = clinicDto.CountryId;
            clinic.PhoneNumber = clinicDto.PhoneNumber;
            clinic.CityId = clinicDto.CityId;
            _unitOfWork.Clinics.Update(clinic);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
        if (clinic == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Clinics.Delete(clinic);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }
}
