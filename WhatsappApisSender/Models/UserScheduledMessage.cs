namespace WhatsappApisSender.Models
{
    public class UserScheduledMessage
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string UserEmail { get; set; }
        public string ToPhoneNumber { get; set; }
        public string RequestBody { get; set; }
        public string Token { get; set; }
        public DateTime DueDateUTC { get; set; }
    }
}
