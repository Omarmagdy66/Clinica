namespace Dto;

public class ReviewDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime? ReviewDate { get; set; } = DateTime.Now.Date;

    //public bool IsFlagged { get; set; } = false;
    //public string? FlagReason { get; set; }
    //public bool IsApproved { get; set; } = true;
    //public int HelpfulCount { get; set; } = 0;
}
