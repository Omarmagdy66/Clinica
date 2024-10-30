using DAL;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Claims;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : APIBaseController
{
    public PatientController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    [HttpGet("GetAll")]
    [Authorize (Roles = "1")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _unitOfWork.Patients.GetAllAsync());
    }


    [HttpGet("GetPatientById")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(patient);
    }


    [HttpPost("AddPatient")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> AddPatient(PatientDto patientdto)
    {
        if (ModelState.IsValid)
        {
            var hashpass = BCrypt.Net.BCrypt.HashPassword(patientdto.Password);
            var user = new User()
            {
                UserName = patientdto.Name,
                Email = patientdto.Email,
                Password = hashpass,
            };
            var patient = new Patient()
            {
                Name = patientdto.Name,
                Email = patientdto.Email,
                Birthday = patientdto.Birthday,
                Gender = patientdto.Gender,
                Password = hashpass,
                PhoneNumber = patientdto.PhoneNumber,
                RegistrationDate = patientdto.RegistrationDate
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Patients.AddAsync(patient);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }


    [HttpPut("EditPatient")]
    [Authorize(Roles = "1,2")]
    public async Task<IActionResult> EditPatient([FromBody] PatientDto patientdto)
    {
        // Get the patient ID from the token claims
        var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (patientIdClaim == null)
        {
            return Unauthorized("Patient ID not found in token.");
        }

        // Parse the patient ID
        int patientId;
        if (!int.TryParse(patientIdClaim, out patientId))
        {
            return BadRequest("Invalid Patient ID in token.");
        }

        // Fetch the patient from the database
        var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
        var user = await _unitOfWork.Users.FindAsync(u=>u.Email == patient.Email);

        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }

        if (ModelState.IsValid)
        {
            // Update only the fields that have been provided in the DTO
            if (!string.IsNullOrEmpty(patientdto.Name) && patient.Name != patientdto.Name)
            {
                var user1 = await _unitOfWork.Users.FindAsync(u => u.UserName == patientdto.Name);
                if (user1 != null)
                    return BadRequest("Name already Exist!");
                patient.Name = patientdto.Name;
                user.UserName = patientdto.Name;
            }

            if (!string.IsNullOrEmpty(patientdto.Email) && patient.Email != patientdto.Email)
            {
                var user1 = await _unitOfWork.Users.FindAsync(u => u.Email == patientdto.Email);
                if (user1 !=null)
                    return BadRequest("Email already Exist!");
                patient.Email = patientdto.Email;
                user.Email = patientdto.Email;
            }

            if (patientdto.Birthday.HasValue && patient.Birthday != patientdto.Birthday)
            {
                patient.Birthday = patientdto.Birthday.Value;
            }

            if (!string.IsNullOrEmpty(patientdto.Gender) && patient.Gender != patientdto.Gender)
            {
                patient.Gender = patientdto.Gender;
            }

            if (!string.IsNullOrEmpty(patientdto.Password) && !BCrypt.Net.BCrypt.Verify(patientdto.Password, patient.Password))
            {
                patient.Password = BCrypt.Net.BCrypt.HashPassword(patientdto.Password);
                user.Password = BCrypt.Net.BCrypt.HashPassword(patientdto.Password);
            }

            if (!string.IsNullOrEmpty(patientdto.PhoneNumber) && patient.PhoneNumber != patientdto.PhoneNumber)
            {
                patient.PhoneNumber = patientdto.PhoneNumber;
            }

            // Update the patient entity in the database
            _unitOfWork.Patients.Update(patient);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            return Ok("Updated!");
        }

        return BadRequest($"There are {ModelState.ErrorCount} errors in the model.");
    }


    [HttpDelete("DeletePatient")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> DeletePatient(int patientId)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Patients.Delete(patient);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }


    // GET: api/patient/search
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PatientDto>>> SearchPatients(
        [FromQuery] string? email = null,
        [FromQuery] string? gender = null,
        [FromQuery] string? name= null,
       // [FromQuery] string? registrationDate = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        Expression<Func<Models.Patient, bool>> criteria = patient =>
            (email == null || patient.Email.Contains(email)) &&
            (gender == null || patient.Gender == gender) &&
         (name == null || patient.Name == name);
        // (gender == null || patient.RegistrationDate == registrationDate);

        var patients = await _unitOfWork.Patients.FindAllAsync(criteria, take, skip);
        return Ok(patients);
    }
    // GET: api/patient/filtered
    [HttpGet("filtered")]
    public ActionResult<IEnumerable<PatientDto>> GetPatientsWithFilters(
        [FromQuery] string? sortColumn = null,
        [FromQuery] string? sortDirection = "asc",
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var patients = _unitOfWork.Patients.FindWithFilters(
            criteria: null,
            sortColumn: sortColumn,
            sortColumnDirection: sortDirection,
            skip: skip,
            take: take);

        return Ok(patients);
        
    }
    // GET: api/patient/last
    [HttpGet("last")]
    public ActionResult<PatientDto> GetLastRegisteredPatient()
    {
        var lastPatient = _unitOfWork.Patients.Last(x => true, x => x.RegistrationDate);
        return Ok(lastPatient);
    }

    // GET: api/patient/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetPatientCount()
    {
        var count = await _unitOfWork.Patients.CountAsync();
        return Ok(count);
    }
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetPatientsByDoctor(int doctorId)
    {
        var appointments = await _unitOfWork.Appointments.FindAllAsync(E => E.DoctorId == doctorId);
        var patientid = appointments.Select(a => a.PatientId).ToList();
        var patients = await _unitOfWork.Patients
                        .FindAllAsync(p => patientid.Contains(p.Id));


        if (patients == null || !patients.Any())
        {
            return NotFound("No patients found for this doctor");
        }
        var patientsName = patients.Select(patients => patients.Name).ToList();
        return Ok(patientsName);
    }





}

/*
      [HttpGet("doctor/{doctorId}")]
       public async Task<IActionResult> GetPatientsByDoctor(int doctorId)
       {
           var appointments= await _unitOfWork.Appointments.FindAllAsync(E=>E.DoctorId==doctorId);
           var patientid = appointments.Select(a => a.PatientId).ToList();
           var patients = await _unitOfWork.Patients
                           .FindAll(p => patientid.Contains(p.PatientId));


           if (patients == null || !patients.Any())
           {
               return NotFound("No patients found for this doctor");
           }
           var patientsName = patients.Select(patients => patients.Name).ToList();
           return Ok(patientsName);
       }*/

/* [HttpGet("{id}/medical-history")]
   public async Task<IActionResult> GetPatientMedicalHistory(int id)
   {
       // Validate the id (optional but recommended)
       if (id <= 0)
       {
           return BadRequest("Invalid patient ID.");
       }

       // Fetch the medical history using existing repository methods
       var history = await _unitOfWork.MedicalHistories
           .FindAllAsync(mh => mh.PatientId == id); // Assuming MedicalHistory has a PatientId property

       // Check if history is null or empty and return NotFound if it is
       if (history == null || !history.Any())
       {
           return NotFound($"Medical history not found for patient ID: {id}");
       }

       // Return the medical history
       return Ok(history);
   }

   [HttpGet("{id}/upcoming-appointments")]
   public async Task<IActionResult> GetUpcomingAppointments(int id)
   {
       var appointments = await _unitOfWork.Patients.GetUpcomingAppointmentsAsync(id);
       if (appointments == null || !appointments.Any())
       {
           return NotFound("No upcoming appointments found for this patient");
       }
       return Ok(appointments);
   }

 // GET: api/patient/max-birthday
    [HttpGet("max-birthday")]
    public async Task<ActionResult<long>> GetMaxBirthday()
    {
        var maxBirthday = await _unitOfWork.Patients.MaxAsync(x => x.Birthday);
        return Ok(maxBirthday);
    }

    // GET: api/patient/max-criteria
    [HttpGet("max-criteria")]
    public async Task<ActionResult<long>> GetMaxBirthdayByCriteria([FromQuery] string? gender = null)
    {
        Expression<Func<Models.Patient, bool>> criteria = patient =>
            gender == null || patient.Gender == gender;

        var maxBirthday = await _unitOfWork.Patients.MaxAsync(criteria, x => x.Birthday);
        return Ok(maxBirthday);
    }

    [HttpGet("distinct/{column}")]
    public ActionResult<IEnumerable<string>> GetDistinctPatients(string column)
    {
        var property = typeof(PatientDto).GetProperty(column);
        if (property == null) return BadRequest("Invalid column name");

        var distinctValues = _unitOfWork.Patients.GetDistinct(x => (string)property.GetValue(x));
        return Ok(distinctValues);
    }
  */
