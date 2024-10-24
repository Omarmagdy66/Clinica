using Controllers;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Cls.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : APIBaseController
    {
        public CountryController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            return Ok(await _unitOfWork.Countries.GetAllAsync());
        }


        [HttpGet("GetCountryById")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);
            if (country == null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(country);
        }


        [HttpPost("AddCountry")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddCountry(CountryDto countryDto)
        {
            if (ModelState.IsValid)
            {
                var country = new Country()
                {
                       Name = countryDto.Name
                };
                await _unitOfWork.Countries.AddAsync(country);
                _unitOfWork.Save();
                return Ok("Created!");
            }
            return BadRequest($"ther are {ModelState.ErrorCount} errors");
        }


        [HttpPut("EditCountry")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> EditCountry(int id, [FromBody] CountryDto CountryDto)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);
            if (country == null)
            {
                return BadRequest("Invalid Id");
            }
            if (ModelState.IsValid)
            {
                country.Name = CountryDto.Name;
                _unitOfWork.Countries.Update(country);
                _unitOfWork.Save();
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
        }


        [HttpDelete("DeleteCountry")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);
            if (country == null)
            {
                return BadRequest("Invalid Id");
            }
            _unitOfWork.Countries.Delete(country);
            _unitOfWork.Save();
            return Ok("Deleted!");
        }
    }
}
