using DAL;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using System.Linq.Expressions;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : APIBaseController
{
    public PatientController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        //var Patients = await _unitOfWork.Patients.GetAllAsync();
        return Ok(await _unitOfWork.Patients.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(patient);
    }
    [HttpPost]
    public async Task<IActionResult> AddPatient(PatientDto patientdto)
    {
        if (ModelState.IsValid)
        {
            var patient = new Patient()
            {
                Name = patientdto.Name,
                Email = patientdto.Email,
                Birthday = DateOnly.Parse(patientdto.Birthday),
                Gender = patientdto.Gender,
                Password = patientdto.Password,
                PhoneNumber = patientdto.PhoneNumber,
                RegistrationDate = DateOnly.Parse(patientdto.RegistrationDate)
            };
            await _unitOfWork.Patients.AddAsync(patient);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditPatient(int id, [FromBody] PatientDto patientdto)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            patient.Name = patientdto.Name;
            patient.Email = patientdto.Email;
            patient.Birthday = DateOnly.Parse(patientdto.Birthday);
            patient.Gender = patientdto.Gender;
            patient.Password = patientdto.Password;
            patient.PhoneNumber = patientdto.PhoneNumber;
            patient.RegistrationDate = DateOnly.Parse(patientdto.RegistrationDate);
            _unitOfWork.Patients.Update(patient);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
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
