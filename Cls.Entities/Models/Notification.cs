

namespace Models
{
    public class Notification
    {
        public int Id { get; set; } 
        public string message { get; set; }  
        public int? Doctorid { get; set; }
        public Doctor? Doctor { get; set; }  
    }
}
