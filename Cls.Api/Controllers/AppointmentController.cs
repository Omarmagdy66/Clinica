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
}
