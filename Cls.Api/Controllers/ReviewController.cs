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
        return Ok(await _unitOfWork.Reviews.GetAllAsync());
    }
    [HttpGet("GetReviewById")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var Review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (Review == null)
        {
            return BadRequest("Invalid Id");
        }
        return Ok(Review);
    }
    [HttpPost("AddReview")]
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
    [HttpPut("EditReview")]
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
            return Ok("Updated Sucsessfully");
        }
        return BadRequest($"There are {ModelState.ErrorCount}");
    }

    [HttpDelete("DeleteReview")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var Review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (Review == null)
        {
            return BadRequest("Invalid Id");
        }
        _unitOfWork.Reviews.Delete(Review);
        _unitOfWork.Save();
        return Ok("Deleted Sucsessfuly");
    }
    [HttpGet("GetReviewsForDoctor")]
    public async Task<IActionResult> GetReviewsForDoctor(int doctorId)
    {
        var reviews = await _unitOfWork.Reviews.FindAllAsync(r => r.DoctorId == doctorId);
        return Ok(reviews);
    }
    [HttpGet("GetAverageRatingForDoctor")]
    public async Task<IActionResult> GetAverageRatingForDoctor(int doctorId)
    {
        var reviews = await _unitOfWork.Reviews.FindAllAsync(r => r.DoctorId == doctorId);
        var averageRating = reviews.Average(r => r.Rating);
        return Ok(new { averageRating });
    }
    [HttpGet("GetReviewsForUser")]
    public async Task<IActionResult> GetReviewsForUser(int patientId)
    {
        var reviews = await _unitOfWork.Reviews.FindAllAsync(r => r.PatientId == patientId);
        return Ok(reviews);
    }


    //[HttpPost("FlagReview")]
    //public async Task<IActionResult> FlagReview(int reviewId, [FromBody] string reason)
    //{
    //    var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
    //    if (review == null)
    //        return NotFound();

    //    review.IsFlagged = true;
    //    review.FlagReason = reason;
    //    _unitOfWork.Reviews.Update(review);
    //    _unitOfWork.Save();

    //    return Ok(new { message = "Review flagged." });
    //}

    [HttpGet("CheckUserReviewedDoctor")]
    public  IActionResult HasUserReviewedDoctor(int patientId, int doctorId)
    {
        var hasReviewed =  _unitOfWork.Reviews.IsExist(r => r.PatientId == patientId && r.DoctorId == doctorId);

        return Ok(new { hasReviewed });
    }
    //[HttpGet("flagged")]
    //public async Task<IActionResult> GetFlaggedReviews()
    //{
    //    var flaggedReviews = await _unitOfWork.Reviews.FindAllAsync(r => r.IsFlagged);
    //    return Ok(flaggedReviews);
    //}

    //[HttpPut("ReviewFlagAction")]
    //public async Task<IActionResult> ReviewFlagAction(int reviewId, [FromBody] string action)
    //{
    //    var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
    //    if (review == null)
    //        return NotFound();

    //    if (action.ToLower() == "approve")
    //    {
    //        review.IsApproved = true;
    //    }
    //    else if (action.ToLower() == "reject")
    //    {
    //        review.IsFlagged = false;
    //        review.FlagReason = null;
    //    }

    //    _unitOfWork.Reviews.Update(review);
    //     _unitOfWork.Save();

    //    return Ok(new { message = "Review flag action processed." });
    //}










}
