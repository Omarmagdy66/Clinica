using DAL;
using Dto;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using SimpleEmailApp.Services.EmailService;
using System.Numerics;
using System.Security.Claims;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class QueryController : APIBaseController
{
    private readonly IEmailService _emailService;
    public QueryController(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
    {
        _emailService = emailService;
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

            var Query = new Query()
            {
                Name = Querydto.Name,
                Email = Querydto.Email,
                QueryText = Querydto.QueryText,
                QueryDate = DateTime.Now.Date,
                QueryStatus = "Pending",
            };
            await _unitOfWork.Queries.AddAsync(Query);
            _unitOfWork.Save();
        try
        {
            var Email = new EmailDto()
            {
                Subject = "We Value Your Feedback on Clinica!",
                Body = $"Dear {Querydto.Name},<br><br>Thank you for using Clinica for your healthcare needs. We are constantly striving to improve our platform, and your opinion matters to us!<br><br>We would appreciate it if you could take a moment to share your feedback about your experience using our site.<br><br>Please click the link below to provide your feedback:<br><a href='[Feedback Link]'>Share Your Feedback</a><br><br>Thank you for helping us enhance our services.<br><br>Best regards,<br>CLINICA Team",
                To = Querydto.Email
            };
            _emailService.SendEmail(Email);
            return Ok("Created!");
        }
        catch (Exception ex)
        {
            return Ok("Created!");
        }
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
            Query.Name = Querydto.Name;
            Query.Email = Querydto.Email;
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
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetQueriesByStatus(string status)
    {
        var queries = await _unitOfWork.Queries.FindAllAsync(q => q.QueryStatus == status);
        return Ok(queries);
    }


    [HttpPut("SolveQuery")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> SolveQuery(int id)
    {
        var query = await _unitOfWork.Queries.GetByIdAsync(id);
        if (query == null)
            return NotFound();

        query.QueryStatus = "Solved";
        _unitOfWork.Queries.Update(query);
        _unitOfWork.Save();
        try
        {
            var Email = new EmailDto()
            {
                Subject = "Your Issue Has Been Resolved",
                Body = $"Dear {query.Name},<br><br>We are pleased to inform you that the issue you reported regarding Clinica has been successfully resolved.<br><br>Thank you for bringing this to our attention. If you experience any further problems or have any other questions, please don't hesitate to contact us.<br><br>We appreciate your feedback and your patience in helping us improve our platform.<br><br>Best regards,<br>Clinica Support Team",
                To = query.Email
            };
            _emailService.SendEmail(Email);
            return Ok("Query Solved");
        }
        catch (Exception ex)
        {
            return Ok("Query Solved");
        }

        
    }


}