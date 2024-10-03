using HospitalAPI.DTO;
using Interfaces;
using Cls.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HospitalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SpecializationController : APIBaseController
	{
		public SpecializationController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
		[HttpGet]
		public async Task<IActionResult> GetAllSpecializations()
		{
			//var Specializations = await _unitOfWork.Specializations.GetAllAsync();
			return Ok(await _unitOfWork.Specializations.GetAllAsync());
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetSpecializationById(int id)
		{
			var department = await _unitOfWork.Specializations.GetByIdAsync(id);
			if (department == null)
			{
				return BadRequest("Invalid Id");
			}
			return Ok(department);
		}
		[HttpPost]
		public async Task<IActionResult> AddSpecialization(SpecializationDto specializationDto)
		{
			if (ModelState.IsValid)
			{
				var specialization = new Specialization()
				{
					  SpecializationName = specializationDto.SpecializationName
                };
				await _unitOfWork.Specializations.AddAsync(specialization);
				_unitOfWork.Save();
				return Ok("Created!");
			}
			return BadRequest($"ther are {ModelState.ErrorCount} errors");
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> EditSpecialization(int id, [FromBody] SpecializationDto specializationDto)
		{
			var specialization = await _unitOfWork.Specializations.GetByIdAsync(id);
			if (specialization == null)
			{
				return BadRequest("Invalid Id");
			}
			if (ModelState.IsValid)
			{
                specialization.SpecializationName = specializationDto.SpecializationName;
                _unitOfWork.Specializations.Update(specialization);
				_unitOfWork.Save();
				return Ok("Updated!");
			}
			return BadRequest($"There are {ModelState.ErrorCount}");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSpecialization(int id)
		{
			var department = await _unitOfWork.Specializations.GetByIdAsync(id);
			if (department == null)
			{
				return BadRequest("Invalid Id");
			}
			_unitOfWork.Specializations.Delete(department);
			_unitOfWork.Save();
			return Ok("Deleted!");
		}
	}
}
