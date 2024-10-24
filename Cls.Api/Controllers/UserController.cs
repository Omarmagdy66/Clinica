using Cls.Api.Dto;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : APIBaseController
{
    private readonly IConfiguration configuration;
    private readonly JwtService jwt;

    public UserController(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
    {
        this.configuration = configuration;
        this.jwt = new JwtService(configuration);
    }
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return Ok(users);

    }
    [HttpGet("GetById")]
    public IActionResult GetById(int id)
    {
        var User = _unitOfWork.Users.GetById(id);
        if (User == null) return BadRequest("Invalid Id");
        return Ok(User);
    }

    [HttpPut("EditPassword")]
    public async Task<IActionResult> EditPassword(int id, UserDto userDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        var patient = await _unitOfWork.Patients.FindAsync(p => p.Email == user.Email);
        if (user == null || patient == null)
        {
            return BadRequest("Invalid Id");
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        _unitOfWork.Users.Update(user);
        _unitOfWork.Patients.Update(patient);
        _unitOfWork.Save();
        return Ok("Updated!");
    }


    [HttpPost("PatientRegister")]
    public async Task<IActionResult> PatientRegister(PatientRegesterDto dto)
    {
        var user1 = await _unitOfWork.Users.FindAsync(u => u.UserName == dto.UserName || u.Email == dto.Email);
        if (user1.UserName == dto.UserName)
            return BadRequest("Username already Exist!");
        else if(user1.Email == dto.Email)
            return BadRequest("Email already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Password = hashedPassword,
            RoleId = 2
        };
        _unitOfWork.Users.Add(user);
        var P = new Patient()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            Birthday = dto.Birthday,
        };
        _unitOfWork.Patients.Add(P);
        _unitOfWork.Save();
        return Ok("Created");
    }

    [HttpPost("DoctorRegister")]
    public async Task<IActionResult> CreateDoctorAsync(DoctorRegisterDto dto)
    {
        var user1 = await _unitOfWork.Users.FindAsync(u => u.UserName == dto.UserName || u.Email == dto.Email);
        if (user1.UserName == dto.UserName)
            return BadRequest("Username already Exist!");
        else if (user1.Email == dto.Email)
            return BadRequest("Email already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Password = hashedPassword,
            RoleId = 3
        };
        _unitOfWork.Users.Add(user);
        var d = new Doctor()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword,
            SpecializationId = dto.SpecializationId,
            PhoneNumber = dto.PhoneNumber,
            Price = dto.Price,
            Bio = dto.Bio,
            Examinationduration = dto.Examinationduration,
            //Image= dto.Image
        };
        _unitOfWork.Doctors.Add(d);
        _unitOfWork.Save();
        return Ok("Created");
    }
    [HttpPost("NurseRegister")]
    public async Task<IActionResult> CreateNurseAsync(NurseRegesterDto dto)
    {
        var user1 = await _unitOfWork.Users.FindAsync(u => u.UserName == dto.UserName || u.Email == dto.Email);
        if (user1 != null)
            return BadRequest("Username already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            UserName = dto.Name,
            Email = dto.Email,
            Password = hashedPassword,
            RoleId = 4
        };
        _unitOfWork.Users.Add(user);
        var d = new Nurse()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword,
            ClinicId = dto.ClinicId,
            PhoneNumber = dto.PhoneNumber,
            Price = dto.Price,
            Bio = dto.Bio,

            Image = dto.Image,
        };
        _unitOfWork.Nurses.Add(d);
        _unitOfWork.Save();
        return Ok("Created");
    }


    [HttpPost("CreateAdmin")]
    public async Task<IActionResult> CreateAdmin(UserDto userDto)
    {
        if (ModelState.IsValid)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var User = new User()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = hashedPassword,
                RoleId = 1
            };
            await _unitOfWork.Users.AddAsync(User);
            var admin = new Admin()
            {
                Name = userDto.UserName,
                Email = userDto.Email,
                Password = hashedPassword,
            };
            await _unitOfWork.Admins.AddAsync(admin);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }


    [HttpPut("Update")]
    public IActionResult Update(int id)
    {
        var user = _unitOfWork.Users.GetById(id);
        if (user == null) return BadRequest();
        _unitOfWork.Users.Update(user);
        _unitOfWork.Save();
        return Ok("Updated");
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var user = _unitOfWork.Users.GetById(id);
        if (user == null) return BadRequest();
        _unitOfWork.Users.Delete(user);
        _unitOfWork.Save();
        return Ok("Deleted");
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAndTokenAsync(LoginDto loginDto)
    {
        if (ModelState.IsValid)
        {
            // Query by either username or email
            User user = await _unitOfWork.Users.FindAsync(u => u.UserName == loginDto.UserName);
            if (user == null)
            {
                user = await _unitOfWork.Users.FindAsync(u => u.Email == loginDto.UserName);
            }

            if (user == null) return BadRequest("Username or Password is incorrect");

            // Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!isPasswordValid) return BadRequest("Username or Password is incorrect");

            // Get the role of the user
            var role = _unitOfWork.Roles.GetById((int)user.RoleId);
            if (role == null) return BadRequest("Invalid role");

            // Role-based logic
            switch (user.RoleId)
            {
                case 1: // Admin
                    var admin = await _unitOfWork.Admins.FindAsync(a => a.Email == user.Email);
                    if (admin == null) return BadRequest("Admin not found");
                    return Ok(new
                    {
                        token = jwt.GenerateJSONWebToken(admin, user.RoleId.ToString(), role.RoleName),
                        expiration = DateTime.UtcNow.AddHours(1)
                    });

                case 2: // Patient
                    var patient = await _unitOfWork.Patients.FindAsync(p => p.Email == user.Email);
                    if (patient == null) return BadRequest("Patient not found");
                    return Ok(new
                    {
                        token = jwt.GenerateJSONWebToken(patient, user.RoleId.ToString(), role.RoleName),
                        expiration = DateTime.UtcNow.AddHours(1)
                    });

                case 3: // Doctor
                    var doctor = await _unitOfWork.Doctors.FindAsync(d => d.Email == user.Email);
                    if (doctor == null) return BadRequest("Doctor not found");
                    return Ok(new
                    {
                        token = jwt.GenerateJSONWebToken(doctor, user.RoleId.ToString(), role.RoleName),
                        expiration = DateTime.UtcNow.AddHours(1)
                    });

                case 4: // Nurse
                    var nurse = await _unitOfWork.Nurses.FindAsync(n => n.Email == user.Email);
                    if (nurse == null) return BadRequest("Nurse not found");
                    return Ok(new
                    {
                        token = jwt.GenerateJSONWebToken(nurse, user.RoleId.ToString(), role.RoleName),
                        expiration = DateTime.UtcNow.AddHours(1)
                    });

                default:
                    return BadRequest("Invalid role");
            }
        }

        return BadRequest(ModelState);
    }
}



