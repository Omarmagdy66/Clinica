using Cls.Api.Dto;
using HospitalAPI.DTO;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HospitalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorScheduleController : APIBaseController
	{
		public DoctorScheduleController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
		[HttpGet]
		public async Task<IActionResult> GetAllSchedules()
		{
			//var Schedules = await _unitOfWork.Schedules.GetAllAsync();
			return Ok(await _unitOfWork.Schedules.GetAllAsync());
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetDoctorScheduleById(int id)
		{
			var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
			if (schedule == null)
			{
				return BadRequest("Invalid Id");
			}
			return Ok(schedule);
		}
		[HttpPost]
		public async Task<IActionResult> AddDoctorSchedule(DoctorScheduleDto scheduledto)
		{
			if (ModelState.IsValid)
			{
				var schedule = new DoctorSchedule()
				{
					 DayOfWeek = scheduledto.DayOfWeek,
					 DoctorId = scheduledto.DoctorId,
					 WorkingHours = scheduledto.WorkingHours
				};
				await _unitOfWork.Schedules.AddAsync(schedule);
				_unitOfWork.Save();
				return Ok("Created!");
			}
			return BadRequest($"ther are {ModelState.ErrorCount} errors");
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> EditDoctorSchedule(int id, [FromBody] DoctorScheduleDto scheduledto)
		{
			var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
			if (schedule == null)
			{
				return BadRequest("Invalid Id");
			}
			if (ModelState.IsValid)
			{
				schedule.DayOfWeek = scheduledto.DayOfWeek;
				schedule.DoctorId = scheduledto.DoctorId;
				schedule.WorkingHours = scheduledto.WorkingHours;
                _unitOfWork.Schedules.Update(schedule);
				_unitOfWork.Save();
				return Ok("Updated!");
			}
			return BadRequest($"There are {ModelState.ErrorCount}");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDoctorSchedule(int id)
		{
			var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
			if (schedule == null)
			{
				return BadRequest("Invalid Id");
			}
			_unitOfWork.Schedules.Delete(schedule);
			_unitOfWork.Save();
			return Ok("Deleted!");
		}
	}
}
