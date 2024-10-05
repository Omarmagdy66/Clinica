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
    public async Task<IActionResult> GetAllLocations()
    {
        //var Clinics = await _unitOfWork.Clinics.GetAllAsync();
        return Ok(await _unitOfWork.Clinics.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var clinic = await _unitOfWork.Clinics.GetByIdAsync(id);
        if (clinic == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(clinic);
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
