using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorScheduleController : APIBaseController
{
    public DoctorScheduleController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }


    [HttpGet("GetAllSchedules")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAllSchedules()
    {
        return Ok(await _unitOfWork.Schedules.GetAllAsync());
    }


    [HttpGet("GetDoctorScheduleById")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetDoctorScheduleById(int id)
    {
        var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
        if (schedule == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(schedule);
    }


    [HttpPost("AddDoctorSchedule")]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> AddDoctorSchedule(DoctorScheduleDto scheduledto)
    {
        if (ModelState.IsValid)
        {
            // Extract DoctorId from token
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in token.");
            }

            if (!int.TryParse(doctorIdClaim, out int doctorId))
            {
                return BadRequest("Invalid Doctor ID in token.");
            }

            // Check if ClinicId and Time slots are provided
            if (scheduledto.ClinicId == null || scheduledto.AvailableFrom == null || scheduledto.AvailableTo == null)
            {
                return BadRequest("ClinicId, AvailableFrom, and AvailableTo fields are required.");
            }

            // Get doctor's examination duration from the database
            var doctor = await _unitOfWork.Doctors.FindAsync(d => d.Id == doctorId);
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            if (!doctor.Examinationduration.HasValue)
            {
                return BadRequest("Doctor's examination duration is not set.");
            }

            int duration = doctor.Examinationduration.Value;

            // Check if there are overlapping schedules for the same doctor on the same day
            var existingSchedules = await _unitOfWork.Schedules.FindAllAsync(s =>
                s.DoctorId == doctorId &&
                s.Day == scheduledto.Day &&
                ((scheduledto.AvailableFrom >= s.AvailableFrom && scheduledto.AvailableFrom < s.AvailableTo) ||
                 (scheduledto.AvailableTo > s.AvailableFrom && scheduledto.AvailableTo <= s.AvailableTo) ||
                 (scheduledto.AvailableFrom <= s.AvailableFrom && scheduledto.AvailableTo >= s.AvailableTo))
            );

            if (existingSchedules.Any())
            {
                return BadRequest("A conflicting schedule exists for the same date and time range.");
            }

            // Initialize the start and end time
            TimeOnly startTime = scheduledto.AvailableFrom.Value;
            TimeOnly endTime = scheduledto.AvailableTo.Value;

            // Create list to hold the schedules
            var schedules = new List<DoctorSchedule>();

            // Generate time slots based on examination duration
            while (startTime < endTime)
            {
                TimeOnly nextSlot = startTime.AddMinutes(duration);
                if (nextSlot > endTime) break;

                var schedule = new DoctorSchedule
                {
                    ClinicId = scheduledto.ClinicId.Value,
                    DoctorId = doctorId,
                    Day = scheduledto.Day.HasValue ? scheduledto.Day.Value : DateTime.Now, // Default to current day if not provided
                    AvailableFrom = startTime,
                    AvailableTo = nextSlot,
                    Status = true
                };

                schedules.Add(schedule);
                startTime = nextSlot; // Move to next time slot
            }

            // Add all schedules to the database
            await _unitOfWork.Schedules.AddRangeAsync(schedules);
            _unitOfWork.Save();

            return Ok("Doctor schedule created successfully!");
        }

        return BadRequest($"There are {ModelState.ErrorCount} errors in the form.");
    }




    //[HttpPut("EditDoctorSchedule")]
    //public async Task<IActionResult> EditDoctorSchedule(int id, [FromBody] DoctorScheduleDto scheduledto)
    //{
    //    var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
    //    if (schedule == null)
    //    {
    //        return BadRequest("Invalid Id");
    //    }
    //    if (ModelState.IsValid)
    //    {
    //        var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    //        if (doctorIdClaim == null)
    //        {
    //            return Unauthorized("Doctor ID not found in token.");
    //        }

    //        // Parse the DoctorId
    //        int doctorId;
    //        if (!int.TryParse(doctorIdClaim, out doctorId))
    //        {
    //            return BadRequest("Invalid Doctor ID in token.");
    //        }
    //        schedule.ClinicId = scheduledto.ClinicId;
    //        schedule.DoctorId = doctorId;
    //        schedule.Day = scheduledto.Day;
    //        schedule.AvailableFrom = scheduledto.AvailableFrom;
    //        schedule.AvailableTo = scheduledto.AvailableTo;
    //        schedule.Status = true;
    //        _unitOfWork.Schedules.Update(schedule);
    //        _unitOfWork.Save();
    //        return Ok("Updated!");
    //    }
    //    return BadRequest($"There are {ModelState.ErrorCount}");
    //}

    [HttpDelete("DeleteDoctorSchedule")]
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


    [HttpGet("GetSchedulesByDoctorId")]
    public async Task<IActionResult> GetSchedulesByDoctorId(int? idDoctor)
    {
        var Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (Role == null)
        {
            return Unauthorized("Doctor ID not found in token.");
        }

        // Parse the DoctorId
        int RoleId;
        if (!int.TryParse(Role, out RoleId))
        {
            return BadRequest("Invalid Doctor ID in token.");
        }
        if(RoleId == 3) {
        // Extract DoctorId from token
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int doctorId))
        {
            return Unauthorized("Doctor ID not found or invalid in token.");
        }
        

        // Fetch doctor's schedules from the database
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.DoctorId == doctorId);

        // If no schedules are found, return NotFound status
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found for Doctor with Id {doctorId}");
        }

        // Return the schedules with OK status
        return Ok(schedules);
        }
        else { 
            // Fetch doctor's schedules from the database
            var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.DoctorId == idDoctor);

        // If no schedules are found, return NotFound status
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found for Doctor with Id {idDoctor}");
        }

        // Return the schedules with OK status
        return Ok(schedules);
        }
    }



    [HttpGet("GetSchedulesByClinicId")]
    public async Task<IActionResult> GetSchedulesByClinicId(int clinicId)
    {
        // Fetch schedules for the specified clinic from the database
        var schedules = await _unitOfWork.Schedules.FindAllAsync(s => s.ClinicId == clinicId);

        // If no schedules are found, return NotFound status
        if (schedules == null || !schedules.Any())
        {
            return NotFound($"No schedules found for Clinic with Id {clinicId}");
        }

        // Return the list of schedules with OK status
        return Ok(schedules);
    }



    [HttpGet("GetSchedulesByDate")]
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


    [HttpGet("GetSchedulesByStatus")]
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
