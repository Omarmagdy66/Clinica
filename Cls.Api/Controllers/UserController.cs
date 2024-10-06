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
    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(UserDto dto)
    {
        var user1 = await _unitOfWork.Users.GetByNameAsync(dto.Name);
        if (user1 != null)
            return BadRequest("Username already Exist!");
        User user = new User()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            RoleId = dto.RoleId,
        };
        _unitOfWork.Users.Add(user);
        _unitOfWork.Save();
        return Ok("Created!");
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
            User user = await _unitOfWork.Users.GetByNameAsync(loginDto.Name);
            if (user == null) return BadRequest("Username Or Password is incorrect");
            if (loginDto.Password != user.Password) return BadRequest("Username Or Password is incorrect");
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
