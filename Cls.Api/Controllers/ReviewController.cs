using Dto;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : APIBaseController
{
    public ReviewController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        //var Reviews = await _unitOfWork.Reviews.GetAllAsync();
        return Ok(await _unitOfWork.Reviews.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var Review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (Review == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(Review);
    }
    [HttpPost]
    public async Task<IActionResult> AddReview(ReviewDto reviewdto)
    {
        if (ModelState.IsValid)
        {
            var Review = new Review()
            {
                PatientId = reviewdto.PatientId,
                DoctorId = reviewdto.DoctorId,
                Rating = reviewdto.Rating,
                ReviewText = reviewdto.ReviewText,
                ReviewDate = reviewdto.ReviewDate,
            };
            await _unitOfWork.Reviews.AddAsync(Review);
            _unitOfWork.Save();
            return Ok("Created!");
        }
        return BadRequest($"ther are {ModelState.ErrorCount} errors");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditReview(int id, [FromBody] ReviewDto reviewdto)
    {
        var Review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (Review == null)
        {
            return BadRequest("Invalid Id");
        }
        if (ModelState.IsValid)
        {
            Review.PatientId = reviewdto.PatientId;
            Review.DoctorId = reviewdto.DoctorId;
            Review.Rating = reviewdto.Rating;
            Review.ReviewText = reviewdto.ReviewText;
            Review.ReviewDate = reviewdto.ReviewDate;
				_unitOfWork.Reviews.Update(Review);
            _unitOfWork.Save();
            return Ok("Updated!");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var Review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (Review == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Reviews.Delete(Review);
        _unitOfWork.Save();
        return Ok("Deleted!");
    }
}
