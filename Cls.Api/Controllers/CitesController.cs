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
    public class CitesController : APIBaseController
    {
        public CitesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        [HttpGet("GetAllCites")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllCites()
        {
            return Ok(await _unitOfWork.Cities.GetAllAsync());
        }


        [HttpGet("GetCityById")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var City = await _unitOfWork.Cities.GetByIdAsync(id);
            if (City == null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(City);
        }


        [HttpPost("AddCity")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddCity(CityDto citydto)
        {
            if (ModelState.IsValid)
            {
                var City = new City()
                {
                    CountryId = citydto.CountryId,
                    Name = citydto.Name,

                };
                await _unitOfWork.Cities.AddAsync(City);
                _unitOfWork.Save();
                return Ok("Created!");
            }
            return BadRequest($"ther are {ModelState.ErrorCount} errors");
        }


        [HttpPut("EditCity")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> EditCity(int id, [FromBody] CityDto citydto)
        {
            var City = await _unitOfWork.Cities.GetByIdAsync(id);
            if (City == null)
            {
                return BadRequest("Invalid Id");
            }
            if (ModelState.IsValid)
            {
                City.CountryId = citydto.CountryId;
                City.Name = citydto.Name;
                _unitOfWork.Cities.Update(City);
                _unitOfWork.Save();
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
        }


        [HttpDelete("DeleteCity")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var City = await _unitOfWork.Cities.GetByIdAsync(id);
            if (City == null)
            {
                return BadRequest("Invalid Id");
            }
            _unitOfWork.Cities.Delete(City);
            _unitOfWork.Save();
            return Ok("Deleted!");
        }

    }
}
