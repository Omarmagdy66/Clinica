using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingController : APIBaseController
{
    public BillingController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAllBillings()
    {
        //var Billings = await _unitOfWork.Billings.GetAllAsync();
        return Ok(await _unitOfWork.Billings.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillingById(int id)
    {
        var billing = await _unitOfWork.Billings.GetByIdAsync(id);
        if (billing == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(billing);
    }
    [HttpPost]
    public async Task<IActionResult> AddBilling(BillingDto billingdto)
    {
        if (ModelState.IsValid)
        {
            var billing = new Billing()
            {
                AppointmentId = billingdto.AppointmentId,
                PaidAmount = billingdto.PaidAmount,
                PatientId = billingdto.PatientId,
                PaymentDate = billingdto.PaymentDate,
                PaymentMethod = billingdto.PaymentMethod,
                TotalAmount = billingdto.TotalAmount,
                DueAmount = billingdto.DueAmount,

            };
            await _unitOfWork.Billings.AddAsync(billing);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditBilling(int id, [FromBody] BillingDto billingdto)
    {
        var billing = await _unitOfWork.Billings.GetByIdAsync(id);
        if (billing == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            billing.AppointmentId = billingdto.AppointmentId;
            billing.PaidAmount = billingdto.PaidAmount;
            billing.PatientId = billingdto.PatientId;
            billing.PaymentDate = billingdto.PaymentDate;
            billing.PaymentMethod = billingdto.PaymentMethod;
            billing.TotalAmount = billingdto.TotalAmount;
            billing.DueAmount = billingdto.DueAmount;
            _unitOfWork.Billings.Update(billing);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBilling(int id)
    {
        var billing = await _unitOfWork.Billings.GetByIdAsync(id);
        if (billing == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Billings.Delete(billing);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }
}
