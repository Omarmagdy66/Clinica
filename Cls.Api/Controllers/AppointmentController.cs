using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : APIBaseController
{
    public AppointmentController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }


    [HttpGet("GetAllAppointments")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAllAppointments()
    {
        return Ok(await _unitOfWork.Appointments.GetAllAsync());
    }


    [HttpGet("GetAppointmentById")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(appointment);
    }


    [HttpPost("AddAppointment")]
    [Authorize(Roles = "2,1")]
    public async Task<IActionResult> AddAppointment(AppointmentDto appointmentdto)
    {
        if (ModelState.IsValid)
        {
            var appointment = new Appointment()
            {
                PatientEmailIN = appointmentdto.PatientEmailIN,
                PatientNameIN = appointmentdto.PatientNameIN,
                PatientNumberIN = appointmentdto.PatientNumberIN,
                Teleconsultation = appointmentdto.Teleconsultation,
                PatientId = appointmentdto.PatientId,
                DoctorId = appointmentdto.DoctorId,
                Status = "booked",
                AppointmentDate = appointmentdto.AppointmentDate,
                TimeSlot = appointmentdto.TimeSlot,
            };
            await _unitOfWork.Appointments.AddAsync(appointment);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }


    [HttpPut("EditAppointment")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> EditAppointment(int id, [FromBody] AppointmentDto appointmentdto)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            appointment.TimeSlot = appointmentdto.TimeSlot;
            appointment.PatientEmailIN = appointmentdto.PatientEmailIN;
            appointment.PatientNameIN = appointmentdto.PatientNameIN;
            appointment.PatientNumberIN = appointmentdto.PatientNumberIN;
            appointment.Teleconsultation = appointmentdto.Teleconsultation;
            appointment.PatientId = appointmentdto.PatientId;
            appointment.DoctorId = appointmentdto.DoctorId;
            appointment.AppointmentDate = appointmentdto.AppointmentDate;
            _unitOfWork.Appointments.Update(appointment);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }



    [HttpDelete("DeleteAppointment")]
    [Authorize(Roles = "2,1")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Appointments.Delete(appointment);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }


    [HttpGet("GetAppointmentsByDoctor")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAppointmentsByDoctor(int? doctorid , string? name)
    {
        var doctor = await _unitOfWork.Doctors.FindAsync(a => a.Id == doctorid || a.Name == name);
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctor.Id);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Doctor with Id {doctorid}");
        }
        return Ok(appointments);
    }


    [HttpGet("GetAppointmentsForDoctor")]
    [Authorize(Roles = "3")] // Assuming role 1 is Admin and 3 is Doctor
    public async Task<IActionResult> GetAppointmentsForDoctor()
    {
        // Extract DoctorId from the token
        var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (doctorIdClaim == null)
        {
            return Unauthorized("Doctor ID not found in token.");
        }

        // Parse the DoctorId
        int doctorId;
        if (!int.TryParse(doctorIdClaim, out doctorId))
        {
            return BadRequest("Invalid Doctor ID in token.");
        }

        // Fetch appointments for the Doctor
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctorId);

        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Doctor with Id {doctorId}");
        }

        return Ok(appointments);
    }


    [HttpGet("GetAppointmentsByPatient")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAppointmentsByPatient(int? Patientid , string name)
    {
        var doctor = await _unitOfWork.Doctors.FindAsync(a => a.Id == Patientid || a.Name == name);
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctor.Id);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Doctor with Id {Patientid}");
        }
        return Ok(appointments);
    }


    [HttpGet("GetAppointmentsForPatient")]
    [Authorize(Roles = "2")]
    public async Task<IActionResult> GetAppointmentsForPatient()
    {
        // Extract DoctorId from the token
        var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (patientIdClaim == null)
        {
            return Unauthorized("Doctor ID not found in token.");
        }

        // Parse the DoctorId
        int patientId;
        if (!int.TryParse(patientIdClaim, out patientId))
        {
            return BadRequest("Invalid Doctor ID in token.");
        }

        // Fetch appointments for the Doctor
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.PatientId == patientId);

        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Doctor with Id {patientId}");
        }

        return Ok(appointments);
    }


    [HttpGet("byDateRange")]
    public async Task<IActionResult> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate)
    {
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found between {startDate} and {endDate}");
        }
        return Ok(appointments);
    }


    [HttpGet("status/{id}")]
    public async Task<IActionResult> GetAppointmentStatus(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }
        return Ok(appointment.Status);
    }
    [HttpGet("history/patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentHistoryByPatient(int patientId)
    {
        var history = await _unitOfWork.Appointments.FindAllAsync(a => a.PatientId == patientId && (a.Status == "completed" || a.Status == "cancelled"));
        return Ok(history);
    }

    [HttpGet("history/doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentHistoryByDoctor(int doctorId)
    {
        var history = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctorId && (a.Status == "completed" || a.Status == "cancelled"));
        return Ok(history);
    }






}
