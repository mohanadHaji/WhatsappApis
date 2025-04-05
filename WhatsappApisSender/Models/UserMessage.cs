namespace WhatsappApisSender.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string RequestBody { get; set; }
        public bool IsSuccessfull { get; set; } = true;
        public bool IsScheduled { get; set; } = false;
    }
}
