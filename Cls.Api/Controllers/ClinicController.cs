using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
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


    [HttpGet("GetAllClinics")]
    public async Task<IActionResult> GetAllClinics()
    {
        return Ok(await _unitOfWork.Clinics.GetAllAsync());
    }


    [HttpGet("GetClinicById")]
    public async Task<IActionResult> GetClinicById(int id)
    {
        var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
        if (clinic == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(clinic);
    }

    [HttpGet("GetClinicByDoctor")]
    public async Task<IActionResult> GetClinicByDoctor(int? doctorId)
    {
        var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (doctorIdClaim == null)
        {
            var clinics = await _unitOfWork.Clinics.FindAllAsync(clinic => clinic.DoctorCLinics.Any(doctor => doctor.DoctorId == doctorId));

            if (!clinics.Any())
            {
                return NotFound("No clinics found that have the provided doctor");
            }
            return Ok(clinics);
        }

        // Parse the DoctorId
        if (!int.TryParse(doctorIdClaim, out int docId))
        {
            return BadRequest("Invalid Doctor ID in token.");
        }
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(docId);
        var user = await _unitOfWork.Users.FindAsync(o => o.Email == doctor.Email);
        if(user.RoleId == 3 && user != null) {
            var clinics = await _unitOfWork.Clinics.FindAllAsync(clinic => clinic.DoctorCLinics.Any(doctor => doctor.DoctorId == docId));

            if (!clinics.Any())
            {
                return NotFound("No clinics found that have the provided doctor");
            }
            return Ok(clinics);
        }
        else
        {
            var clinics = await _unitOfWork.Clinics.FindAllAsync(clinic => clinic.DoctorCLinics.Any(doctor => doctor.DoctorId == doctorId));

            if (!clinics.Any())
            {
                return NotFound("No clinics found that have the provided doctor");
            }
            return Ok(clinics);
            
        }
    }











    //[HttpGet("GetClinicByCityId")]
    //public async Task<IActionResult> GetClinicByCityId(int CityId)
    //{
    //    var clinics = await _unitOfWork.Clinics.GetAllAsync();
    //    var clinic = clinics.FirstOrDefault(x => x.CityId == CityId);
    //    if (clinic == null)
    //    {
    //        return BadRequest($"there isn't clinic in the city with id {CityId}");
    //    }
    //    return Ok(clinic);
    //}
    //[HttpGet("GetClinicByCountryId")]
    //public async Task<IActionResult> GetClinicByCountryId(int CountryId)
    //{
    //    var clinics = await _unitOfWork.Clinics.GetAllAsync();
    //    var clinic = clinics.FirstOrDefault(x => x.CountryId == CountryId);
    //    if (clinic == null)
    //    {
    //        return BadRequest($"there isn't clinic in the city with id {CountryId}");
    //    }
    //    return Ok(clinic);
    //}


    //Merge GetClinicByCityId & GetClinicByCountryId
    [HttpGet("GetClinicByCountryIdandCityId")]
    public async Task<IActionResult> GetClinicByCountryIdandCityId(int? CountryId, int? CityId)
    {
        var clinics = await _unitOfWork.Clinics.GetAllAsync();
        if(CityId!=null && CountryId!=null)
        {
            var clinic = clinics.Select(x => x.CountryId == CountryId && x.CityId == CityId);
            if (clinic == null)
            {
                return BadRequest($"there isn't clinic in the city with id {CountryId}");
            }
        return Ok(clinic);
        }
        else if(CountryId!=null)
        {
            var clinic = clinics.FirstOrDefault(x => x.CountryId == CountryId);
            if (clinic == null)
            {
                return BadRequest($"there isn't clinic in the city with id {CountryId}");
            }
            return Ok(clinic);
        }
        else if (CityId != null)
        {
            var clinic = clinics.FirstOrDefault(x => x.CityId == CityId);
            if (clinic == null)
            {
                return BadRequest($"there isn't clinic in the city with id {CityId}");
            }
            return Ok(clinic);
        }
        return Ok(clinics);
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
