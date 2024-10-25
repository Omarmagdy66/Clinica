using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Numerics;


namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : APIBaseController
{
    private readonly IEmailService _emailService;
    public AppointmentController(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
    {
        _emailService = emailService;
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

    //not handel with patient
    [HttpPost("AddAppointment")]
    [Authorize(Roles = "2")]

    public async Task<IActionResult> AddAppointment(AppointmentDto appointmentdto)
    {
        if (ModelState.IsValid)
        {
            // Extract patient ID from the token
            var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (patientIdClaim == null)
            {
                return Unauthorized("Patient ID not found in token.");
            }

            if (!int.TryParse(patientIdClaim, out int patientId))
            {
                return BadRequest("Invalid Patient ID in token.");
            }

            // Get the patient details from the database
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            // Check if the time slot is available in the doctor's schedule and the status is 'true'
            var schedule = await _unitOfWork.Schedules.FindAsync(s =>
                s.DoctorId == appointmentdto.DoctorId &&
                s.Day == appointmentdto.AppointmentDate.Date &&  // Ensure the same day
                s.AvailableFrom <= appointmentdto.TimeSlot && s.AvailableTo > appointmentdto.TimeSlot &&  // Ensure the time slot matches
                s.Status == true);  // Check if the slot is available

            if (schedule == null)  // If no available schedule is found, return error
            {
                return BadRequest("This appointment time is not available or already booked.");
            }

            // Create the appointment entity
            var appointment = new Appointment()
            {
                PatientEmailIN = patient.Email,
                PatientNameIN = patient.Name,
                PatientNumberIN = patient.PhoneNumber,
                Teleconsultation = appointmentdto.Teleconsultation,
                PatientId = patientId,
                DoctorId = appointmentdto.DoctorId,
                Status = "Booked",
                AppointmentDate = appointmentdto.AppointmentDate,
                TimeSlot = appointmentdto.TimeSlot,
            };

            // Save the appointment to the database
            await _unitOfWork.Appointments.AddAsync(appointment);

            // Mark the time slot in the doctor's schedule as 'booked' (status = false)
            schedule.Status = false;
            _unitOfWork.Schedules.Update(schedule);

            // Save all changes to the database
            _unitOfWork.Save();

            var doctor = await _unitOfWork.Doctors.GetByIdAsync((int)appointmentdto.DoctorId);
            var clinic = await _unitOfWork.Clinics.FindAsync(c => c.DoctorCLinics.Any(d => d.DoctorId == doctor.Id));

            // Send Email
            var Email = new EmailDto()
            { 
              Subject = "Appointment Confirmation",
                Body = $"Dear {patient.Name},<br><br>Your appointment with Dr. {doctor.Name} has been confirmed for {appointmentdto.AppointmentDate.ToString("dd/MM/yyyy")} at {appointmentdto.TimeSlot}. Please ensure you arrive on time.<br><br>Thank you for choosing Clinica.<br><br>Best regards,<br>Care Clinic",
                To = patient.Email
            }; 
            _emailService.SendEmail(Email);  


            return Ok("Appointment created and schedule updated successfully!");
        }

        return BadRequest($"There are {ModelState.ErrorCount} errors in the form.");
    }


    //ClinicId Not exist but it run successfully
    //public async Task<IActionResult> AddAppointment(AppointmentDto appointmentdto)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        // Extract patient ID from the token
    //        var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    //        if (patientIdClaim == null)
    //        {
    //            return Unauthorized("Patient ID not found in token.");
    //        }

    //        if (!int.TryParse(patientIdClaim, out int patientId))
    //        {
    //            return BadRequest("Invalid Patient ID in token.");
    //        }

    //        // Get the patient details from the database
    //        var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
    //        if (patient == null)
    //        {
    //            return NotFound("Patient not found.");
    //        }

    //        // Create the appointment entity
    //        var appointment = new Appointment()
    //        {
    //            PatientEmailIN = patient.Email,
    //            PatientNameIN = patient.Name,
    //            PatientNumberIN = patient.PhoneNumber,
    //            Teleconsultation = appointmentdto.Teleconsultation,
    //            PatientId = patientId,
    //            DoctorId = appointmentdto.DoctorId,
    //            Status = "booked",
    //            AppointmentDate = appointmentdto.AppointmentDate,
    //            TimeSlot = appointmentdto.TimeSlot,
    //        };

    //        // Save the appointment to the database
    //        await _unitOfWork.Appointments.AddAsync(appointment);

    //        // Find the doctor's schedule for the same day and time slot
    //        var schedule = await _unitOfWork.Schedules.FindAsync(s =>
    //            s.DoctorId == appointmentdto.DoctorId &&
    //            s.Day == appointmentdto.AppointmentDate.Date && // Ensure same day
    //            s.AvailableFrom <= appointmentdto.TimeSlot && s.AvailableTo > appointmentdto.TimeSlot); // Ensure same time slot

    //        if (schedule == null)   
    //        {
    //            return BadRequest("No available schedule found for this doctor at the selected time.");
    //        }

    //        // Update the schedule status to 'false' (booked)
    //        schedule.Status = false;
    //        _unitOfWork.Schedules.Update(schedule);

    //        // Save all changes to the database
    //        _unitOfWork.Save();

    //        return Ok("Appointment created and schedule updated successfully!");
    //    }

    //    return BadRequest($"There are {ModelState.ErrorCount} errors in the form.");
    //}



    [HttpPut("EditAppointment")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> EditAppointment(int id, [FromBody] AppointmentDto appointmentdto)
    {
        // Extract patient ID from the token
        var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (patientIdClaim == null)
        {
            return Unauthorized("Patient ID not found in token.");
        }

        if (!int.TryParse(patientIdClaim, out int patientId))
        {
            return BadRequest("Invalid Patient ID in token.");
        }

        // Get the patient details from the database
        var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            appointment.TimeSlot = appointmentdto.TimeSlot;
            appointment.PatientEmailIN = patient.Email;
            appointment.PatientNameIN = patient.Name;
            appointment.PatientNumberIN = patient.PhoneNumber;
            appointment.Teleconsultation = appointmentdto.Teleconsultation;
            appointment.PatientId = patientId;
            appointment.DoctorId = appointmentdto.DoctorId;
            appointment.AppointmentDate = appointmentdto.AppointmentDate;
            _unitOfWork.Appointments.Update(appointment);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }


    //[HttpDelete("DeleteAppointment")]
    //[Authorize(Roles = "2,1")]
    //public async Task<IActionResult> DeleteAppointment(int id)
    //{
    //    var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
    //    appointment.Status = "Cancelld";
    //    _unitOfWork.Appointments.Update(appointment);

    //    var schedule = await _unitOfWork.Schedules.FindAsync(s =>s.DoctorId == appointment.DoctorId && s.Day == appointment.AppointmentDate && s.AvailableFrom == appointment.TimeSlot);
    //    if (schedule == null)
    //    {
    //        return BadRequest("No available schedule found for this doctor at the selected time.");
    //    }

    //    schedule.Status = true;
    //    _unitOfWork.Schedules.Update(schedule);
    //    _unitOfWork.Save();
    //    return Ok("Deleted!");
    //}


    [HttpPut("CancelAppointment")]
    [Authorize(Roles = "2")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return BadRequest("Not Found");
        }
        if (appointment.Status == "Booked" || appointment.Status == "booked")
        {
            appointment.Status = "Canceled";
            _unitOfWork.Appointments.Update(appointment);
            var schedule = await _unitOfWork.Schedules.FindAsync(s =>
                    s.DoctorId == appointment.DoctorId &&
                    s.Day == appointment.AppointmentDate.Value.Date &&  // Ensure the same day
                    s.AvailableFrom <= appointment.TimeSlot && s.AvailableTo > appointment.TimeSlot);
            schedule.Status = true;
            _unitOfWork.Schedules.Update(schedule);
            _unitOfWork.Save();
            var patient = await _unitOfWork.Patients.GetByIdAsync(appointment.PatientId);
            var doctor = await _unitOfWork.Doctors.GetByIdAsync((int)appointment.DoctorId);
            var clinic = await _unitOfWork.Clinics.FindAsync(c=>c.Id == schedule.ClinicId);
            var Email = new EmailDto()
            {
                Subject = "Appointment Cancellation Notice",
                Body = $"Dear {patient.Name},<br><br>We regret to inform you that your appointment with Dr. {doctor.Name}, scheduled for {appointment.AppointmentDate.Value.ToString("dd/MM/yyyy")} at {appointment.TimeSlot}, has been cancelled. We apologize for any inconvenience this may cause.<br><br>Please contact us if you wish to reschedule or need further assistance.<br><br>Thank you for your understanding.<br><br>Best regards,<br>{clinic.ClinicName} Clinic",
                To = patient.Email
            };
            _emailService.SendEmail(Email);
            return Ok("Appointment Canceled Successfully");
        }
        else
        { return BadRequest("can't cancelled this appointment it is Completed already"); }
    }


    [HttpGet("GetAppointmentsByDoctor")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAppointmentsByDoctor(int? doctorid, string? name)
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
        var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

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
    public async Task<IActionResult> GetAppointmentsByPatient(int? Patientid, string? name)
    {
        var patient = await _unitOfWork.Patients.FindAsync(a => a.Id == Patientid || a.Name == name);
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.PatientId == patient.Id);
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
        var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

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

    //Not Using
    [HttpGet("GetAppointmentsByDateRange")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate)
    {
        var appointments = await _unitOfWork.Appointments.FindAllAsync(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate);
        if (appointments == null || !appointments.Any())
        {
            return NotFound($"No appointments found between {startDate} and {endDate}");
        }
        return Ok(appointments);
    }



    [HttpPut("EditAppointmentStatus")]
    public async Task<IActionResult> EditAppointmentStatus(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }
        appointment.Status = "Completed";
        _unitOfWork.Appointments.Update(appointment);
        _unitOfWork.Save();
        var schedule = await _unitOfWork.Schedules.FindAsync(s =>
        s.DoctorId == appointment.DoctorId &&
        s.Day == appointment.AppointmentDate.Value.Date &&  // Ensure the same day
        s.AvailableFrom <= appointment.TimeSlot && s.AvailableTo > appointment.TimeSlot);
        var patient = await _unitOfWork.Patients.GetByIdAsync(appointment.PatientId);
        var doctor = await _unitOfWork.Doctors.GetByIdAsync((int)appointment.DoctorId);
        var clinic = await _unitOfWork.Clinics.FindAsync(c => c.Id == schedule.ClinicId);
        var Email = new EmailDto()
        {
            Subject = $"Appointment Completed: Dr. {doctor.Name}, {appointment.AppointmentDate.Value.ToString("dd / MM / yyyy")}",
            Body = $"Dear {patient.Name},<br><br>We hope your appointment with Dr. {doctor.Name} on {appointment.AppointmentDate.Value.ToString("dd/MM/yyyy")} at {appointment.TimeSlot} was successful.<br><br>If you have any feedback or further questions, please don't hesitate to reach out to us.<br><br>Thank you for choosing Clinica, and we look forward to serving you again in the future.<br><br>Best regards,<br>{clinic.ClinicName} Clinic",
            To = patient.Email
        };
        _emailService.SendEmail(Email);
        return Ok("appointment Completed Successfully");
    }

    //Like GetAppointmentsForPatient
    [HttpGet("history/patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentHistoryByPatient(int patientId)
    {
        var history = await _unitOfWork.Appointments.FindAllAsync(a => a.PatientId == patientId && (a.Status == "completed" || a.Status == "cancelled"));
        return Ok(history);
    }

    //Like GetAppointmentsForDoctor
    [HttpGet("history/doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentHistoryByDoctor(int doctorId)
    {
        var history = await _unitOfWork.Appointments.FindAllAsync(a => a.DoctorId == doctorId && (a.Status == "completed" || a.Status == "cancelled"));
        return Ok(history);
    }


}
