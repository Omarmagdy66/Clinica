using Dto;
using HospitalAPI.Controllers;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Cls.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : APIBaseController
    {
        public UserController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
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
        [HttpPost("Create")]
        public IActionResult Create(UserDto dto)
        {
            User user = new User()
            {
                 Name = dto.Name,
                 Email = dto.Email,
                 Password = dto.Password,
                 RoleId = dto.RoleId,
            };
            _unitOfWork.Users.Add(user);
            return Ok();
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

    }
}
