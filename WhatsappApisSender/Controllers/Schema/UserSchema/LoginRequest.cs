namespace WhatsappApisSender.Controllers.Schema.UserSchema
{
    public class LoginRequest
    {
        public required string Email { get; init; }

        public required string Password { get; init; }
    }
}
