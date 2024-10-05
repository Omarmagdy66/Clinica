namespace Dto;

public class QueryDto
{
    public int PatientId { get; set; }
    public int UserId { get; set; }
    public string? QueryText { get; set; }
    public string? QueryStatus { get; set; } // "open", "resolved"
    public DateOnly QueryDate { get; set; }
}
