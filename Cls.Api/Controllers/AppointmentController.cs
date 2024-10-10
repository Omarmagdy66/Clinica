using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : APIBaseController
{
    public AppointmentController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        //var Appointments = await _unitOfWork.Appointments.GetAllAsync();
        return Ok(await _unitOfWork.Appointments.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(appointment);
    }
    [HttpPost]
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
                Status = appointmentdto.Status,
                AppointmentDate = appointmentdto.AppointmentDate,
                TimeSlot = appointmentdto.TimeSlot,
            };
            await _unitOfWork.Appointments.AddAsync(appointment);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
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
            appointment.Status = appointmentdto.Status;
            appointment.AppointmentDate = appointmentdto.AppointmentDate;
            _unitOfWork.Appointments.Update(appointment);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
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
    [HttpGet("byDoctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
    {
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctorId);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Doctor with Id {doctorId}");
        }
        return Ok(appointments);
    }
    [HttpGet("byPatient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
    {
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.PatientId == patientId);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found for Patient with Id {patientId}");
        }
        return Ok(appointments);
    }
    [HttpGet("byDateRange")]
    public async Task<IActionResult> GetAppointmentsByDateRange(DateOnly startDate, DateOnly endDate)
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
