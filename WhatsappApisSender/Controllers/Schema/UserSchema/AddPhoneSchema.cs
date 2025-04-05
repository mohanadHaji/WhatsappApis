namespace WhatsappApisSender.Controllers.Schema.UserSchema
{
    public class AddPhoneSchema
    {
        public List<UserContactInfo> Contacts { get; set; }
    }

    public class UserContactInfo
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
    }
}
