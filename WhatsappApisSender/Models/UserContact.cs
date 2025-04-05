namespace WhatsappApisSender.Models
{
    public class UserContact
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
    }
}
