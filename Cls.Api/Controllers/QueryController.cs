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
    public class QueryController : APIBaseController
    {
        public QueryController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQueries()
        {
            //var Queries = await _unitOfWork.Queries.GetAllAsync();
            return Ok(await _unitOfWork.Queries.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQueryById(int id)
        {
            var Query = await _unitOfWork.Queries.GetByIdAsync(id);
            if (Query == null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(Query);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuery(QueryDto Querydto)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> EditQuery(int id, [FromBody] QueryDto Querydto)
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
                return Ok("Updated!");
            }
            return BadRequest($"There are {ModelState.ErrorCount}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuery(int id)
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
    }
}
