namespace Cls.Api.Dto
{
    public class ReviewDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateOnly? ReviewDate { get; set; }
    }
}
