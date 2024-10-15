using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorScheduleController : APIBaseController
{
    public DoctorScheduleController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    [Authorize(Roles = "Doctor")]
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
                 ClinicId= scheduledto.ClinicId,    
                DoctorId = scheduledto.DoctorId,
                Day = scheduledto.Day,
                AvailableFrom = scheduledto.AvailableFrom,
                AvailableTo = scheduledto.AvailableTo,
                Status = scheduledto.Status,
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
            schedule.ClinicId = scheduledto.ClinicId;
            schedule.DoctorId = scheduledto.DoctorId;
            schedule.Day = scheduledto.Day;
            schedule.AvailableFrom = scheduledto.AvailableFrom;
            schedule.AvailableTo = scheduledto.AvailableTo;
            schedule.Status = scheduledto.Status;
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
    [HttpGet("ByDoctor/{doctorId}")]
    public async Task<IActionResult> GetSchedulesByDoctorId(int doctorId)
    {
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.DoctorId == doctorId);

        if (schedules == null || !schedules.Any())  // Check for null or empty list
        {
            return NotFound($"No schedules found for Doctor with Id {doctorId}");
        }

        return Ok(schedules);  // Return the list of schedules
    }


    [HttpGet("ByClinic/{clinicId}")]
    public async Task<IActionResult> GetSchedulesByClinicId(int clinicId)
    {
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.ClinicId == clinicId);
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found for Clinic with Id {clinicId}");
        }
        return Ok(schedules);
    }

    [HttpGet("ByDate/{date}")]
    public async Task<IActionResult> GetSchedulesByDate(DateTime date)
    {
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.Day == date);
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found on {date}");
        }
        return Ok(schedules);
    }

    [HttpGet("CheckAvailability")]
    public async Task<IActionResult> CheckAvailability(int doctorId, int clinicId, DateTime date, TimeOnly time)
    {
        var schedule = await _unitOfWork.Schedules.FindAsync(s =>
            s.DoctorId == doctorId &&
            s.ClinicId == clinicId &&
            s.Day == date &&
            s.AvailableFrom <= time &&
            s.AvailableTo >= time &&
            s.Status == true);

        if (schedule == null)
        {
            return NotFound($"Doctor is not available at the specified date and time.");
        }
        return Ok("Doctor is available");
    }
    [HttpGet("FilterByStatus/{status}")]
    public async Task<IActionResult> GetSchedulesByStatus(bool status)
    {
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.Status == status);
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found with status: {(status ? "Available" : "Booked")}");
        }
        return Ok(schedules);
    }






}
