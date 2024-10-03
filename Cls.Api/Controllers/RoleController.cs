using Cls.Api.Dto;
using HospitalAPI.DTO;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HospitalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : APIBaseController
	{
		public RoleController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
		[HttpGet]
		public async Task<IActionResult> GetAllRoles()
		{
			//var Roles = await _unitOfWork.Roles.GetAllAsync();
			return Ok(await _unitOfWork.Roles.GetAllAsync());
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetRoleById(int id)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(id);
			if (role == null)
			{
				return BadRequest("Invalid Id");
			}
			return Ok(role);
        }
        [HttpPost]
		public async Task<IActionResult> AddRole(RoleDto roledto)
		{
			if (ModelState.IsValid)
			{
				var role = new Role()
				{
					  RoleName = roledto.RoleName
				};
				await _unitOfWork.Roles.AddAsync(role);
				_unitOfWork.Save();
				return Ok("Created!");
			}
			return BadRequest($"ther are {ModelState.ErrorCount} errors");
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> EditRole(int id, [FromBody] RoleDto roledto)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(id);
			if (role == null)
			{
				return BadRequest("Invalid Id");
			}
			if (ModelState.IsValid)
			{
				role.RoleName = roledto.RoleName;
                _unitOfWork.Roles.Update(role);
				_unitOfWork.Save();
				return Ok("Updated!");
			}
			return BadRequest($"There are {ModelState.ErrorCount}");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRole(int id)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(id);
			if (role == null)
			{
				return BadRequest("Invalid Id");
			}
			_unitOfWork.Roles.Delete(role);
			_unitOfWork.Save();
			return Ok("Deleted!");
		}
	}
}
