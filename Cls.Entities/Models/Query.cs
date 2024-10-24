
namespace Models;

public class Query
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? QueryText { get; set; }
    public string? QueryStatus { get; set; } // "open", "resolved"
    public DateTime QueryDate { get; set; } = DateTime.Now.Date;

}
