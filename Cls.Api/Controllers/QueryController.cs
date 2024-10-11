using DAL;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class QueryController : APIBaseController
{
    public QueryController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _unitOfWork.Queries.GetAllAsync());
    }
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var Query = await _unitOfWork.Queries.GetByIdAsync(id);
        if (Query == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(Query);
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(QueryDto Querydto)
    {
        if (ModelState.IsValid)
        {
            var Query = new Query()
            {
                PatientId = Querydto.PatientId,
                UserId = Querydto.UserId,
                QueryDate = Querydto.QueryDate,
                QueryStatus = Querydto.QueryStatus,
                QueryText = Querydto.QueryText,
            };
            await _unitOfWork.Queries.AddAsync(Query);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("UpdateById")]
    public async Task<IActionResult> Update(int id, [FromBody] QueryDto Querydto)
    {
        var Query = await _unitOfWork.Queries.GetByIdAsync(id);
        if (Query == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            Query.PatientId = Querydto.PatientId;
            Query.UserId = Querydto.UserId;
            Query.QueryDate = Querydto.QueryDate;
            Query.QueryStatus = Querydto.QueryStatus;
            Query.QueryText = Querydto.QueryText;
            _unitOfWork.Queries.Update(Query);
            _unitOfWork.Save();
            return Ok("Updated Sucessfully");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var Query = await _unitOfWork.Queries.GetByIdAsync(id);
        if (Query == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Queries.Delete(Query);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }

    [HttpGet("GetQueriesByStatus")]
    public async Task<IActionResult> GetQueriesByStatus(string status)
    {
        var queries = await _unitOfWork.Queries.FindAllAsync(q => q.QueryStatus == status);
        return Ok(queries);
    }
    [HttpPut("ResolveQuery")]
    public async Task<IActionResult> ResolveQuery(int id)
    {
        var query = await _unitOfWork.Queries.GetByIdAsync(id);
        if (query == null)
            return NotFound();

        query.QueryStatus = "resolved";
        _unitOfWork.Queries.Update(query);
        _unitOfWork.Save();

        return Ok(new { message = "Query resolved." });
    }


}