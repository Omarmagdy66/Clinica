

namespace Models
{
    public class NurseNotification
    {
        public int Id { get; set; }
        public string message { get; set; }
        public int? Nurseid { get; set; }
        public Nurse? Nurse { get; set; }
    }
}
