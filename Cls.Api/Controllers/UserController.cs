﻿using Cls.Api.Dto;
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

    public UserController(IUnitOfWork unitOfWork,IConfiguration configuration) : base(unitOfWork)
    {
        this.configuration = configuration;
        this.jwt = new JwtService(configuration);
    }
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        _unitOfWork.Users.GetAll();
        return Ok();

    }
    [HttpGet("GetById")]
    public IActionResult GetAllPatiant(int id)
    {
       var User = _unitOfWork.Users.GetById(id);
        if(User == null) return BadRequest();
        return Ok();
    }
    [HttpPost("PatientRegister")]
    public async Task<IActionResult> CreateAsync(PatientRegesterDto dto)
    {
        var user1 = await _unitOfWork.Users.GetByNameAsync(dto.UserName);
        if (user1 != null)
            return BadRequest("Username already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            Name = dto.UserName,
            Email = dto.Email,
            Password =hashedPassword,
            RoleId = 2
        };
        _unitOfWork.Users.Add(user);
        var P = new Patient()
        {
            Name = dto.Name,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            Birthday = dto.Birthday,
            //RegistrationDate = DateTime.Now
             
        };
        _unitOfWork.Patients.Add(P);
        _unitOfWork.Save();
        return Ok("Created");
    }
    
    [HttpPost("DoctorRegister")]
    public async Task<IActionResult> CreateDoctorAsync(DoctorRegisterDto dto)
    {
        var user1 = await _unitOfWork.Users.GetByNameAsync(dto.UserName);
        if (user1 != null)
            return BadRequest("Username already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            Name = dto.UserName,
            Email = dto.Email,
            Password =hashedPassword,
            RoleId = 3
        };
        _unitOfWork.Users.Add(user);
        var d = new Doctor()
        {
            Name = dto.Name,
            SpecializationId=dto.SpecializationId,
            PhoneNumber = dto.PhoneNumber,
            Price= dto.Price,
            Bio=dto.Bio,
            Examinationduration=dto.Examinationduration,
            Image= dto.Image
            //RegistrationDate = DateTime.Now
        };
        _unitOfWork.Doctors.Add(d);
        _unitOfWork.Save();
        return Ok("Created");
    }
    [HttpPost("NurseRegister")]
    public async Task<IActionResult> CreateNurseAsync(DoctorRegisterDto dto)
    {
        var user1 = await _unitOfWork.Users.GetByNameAsync(dto.UserName);
        if (user1 != null)
            return BadRequest("Username already Exist!");
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        User user = new User()
        {
            Name = dto.UserName,
            Email = dto.Email,
            Password = hashedPassword,
            RoleId = 4
        };
        _unitOfWork.Users.Add(user);
        var d = new Doctor()
        {
            Name = dto.Name,
            SpecializationId = dto.SpecializationId,
            PhoneNumber = dto.PhoneNumber,
            Price = dto.Price,
            Bio = dto.Bio,
            Examinationduration = dto.Examinationduration,
            Image = dto.Image,
            //RegistrationDate = DateTime.Now
        };
        _unitOfWork.Doctors.Add(d);
        _unitOfWork.Save();
        return Ok("Created");
    }


    [HttpPut("Update")]
    public IActionResult Update(int id)
    {
        var user = _unitOfWork.Users.GetById(id);
        if(user == null) return BadRequest();
        _unitOfWork.Users.Update(user);
        return Ok();
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var user = _unitOfWork.Users.GetById(id);
        if(user == null) return BadRequest();
        _unitOfWork.Users.Delete(user);
        return Ok();
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAndTokenAsync(LoginDto loginDto)
    {
        if (ModelState.IsValid)
        {
            // Query by either username or email
            User user = await _unitOfWork.Users.FindAsync( u => u.Name == loginDto.Name );
            if (user == null)
            {
                user= await _unitOfWork.Users.FindAsync(u=>u.Email == loginDto.Name);
            }

            if (user == null) return BadRequest("Username or Password is incorrect");
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);


            if (!isPasswordValid) return BadRequest("Username or Password is incorrect");

            var role = _unitOfWork.Roles.GetById((int)user.RoleId);

            return Ok(new
            {
                token = jwt.GenerateJSONWebToken(user, role.RoleName),
                expiration = DateTime.UtcNow.AddHours(1)
            });
        }
        return BadRequest(ModelState);
    }


}
