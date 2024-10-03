
namespace Models;

public class Query
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int UserId { get; set; }
    public string? QueryText { get; set; }
    public string? QueryStatus { get; set; } // "open", "resolved"
    public DateOnly QueryDate { get; set; }

    public Patient? Patient { get; set; } 
    public User? User { get; set; } 
}
