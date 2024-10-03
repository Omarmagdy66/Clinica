using Cls.Api.Dto;
using HospitalAPI.DTO;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics.Metrics;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : APIBaseController
    {
        public LocationController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            //var Locations = await _unitOfWork.Locations.GetAllAsync();
            return Ok(await _unitOfWork.Locations.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var clinic = await _unitOfWork.Locations.GetByIdAsync(id);
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
                    Address = clinicDto.Address,
                    ClinicName = clinicDto.ClinicName,
                    Country = clinicDto.Country,
                    PhoneNumber = clinicDto.PhoneNumber,
                    City = clinicDto.City,
                };
                await _unitOfWork.Locations.AddAsync(clinic);
                _unitOfWork.Save();
                return Ok("Created!");
            }
            return BadRequest($"ther are {ModelState.ErrorCount} errors");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLocation(int id, [FromBody] ClinicDto clinicDto)
        {
            var clinic = await _unitOfWork.Locations.GetByIdAsync(id);
            if (clinic == null)
            {
                return BadRequest("Invalid Id");
            }
            if (ModelState.IsValid)
            {
                clinic.Address = clinicDto.Address;
                clinic.ClinicName = clinicDto.ClinicName;
                clinic.Country = clinicDto.Country;
                clinic.PhoneNumber = clinicDto.PhoneNumber;
                clinic.City = clinicDto.City;
                _unitOfWork.Locations.Update(clinic);
                _unitOfWork.Save();
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var clinic = await _unitOfWork.Locations.GetByIdAsync(id);
            if (clinic == null)
            {
                return BadRequest("Invalid Id");
            }
            _unitOfWork.Locations.Delete(clinic);
            _unitOfWork.Save();
            return Ok("Deleted!");
        }
    }
}
