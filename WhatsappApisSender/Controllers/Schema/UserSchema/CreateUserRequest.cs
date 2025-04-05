using System.ComponentModel.DataAnnotations;

namespace WhatsappApisSender.Controllers.Schema.UserSchema
{
    public class CreateUserRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Username { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(this.Email))
                errors.Add("Email is required");
            else if (this.Email.Length > 100)
                errors.Add("Email cannot exceed 100 characters");
            else if (!new EmailAddressAttribute().IsValid(this.Email))
                errors.Add("A valid email is required");

            if (string.IsNullOrWhiteSpace(this.Password))
                errors.Add("Password is required");
            else
            {
                if (this.Password.Length < 8)
                    errors.Add("Password must be at least 8 characters");
                if (this.Password.Length > 100)
                    errors.Add("Password cannot exceed 100 characters");
                if (!this.Password.Any(char.IsUpper))
                    errors.Add("Password must contain at least one uppercase letter");
                if (!this.Password.Any(char.IsLower))
                    errors.Add("Password must contain at least one lowercase letter");
                if (!this.Password.Any(char.IsDigit))
                    errors.Add("Password must contain at least one number");
                if (!this.Password.Any(c => !char.IsLetterOrDigit(c)))
                    errors.Add("Password must contain at least one special character");
            }

            if (string.IsNullOrWhiteSpace(this.Username))
                errors.Add("Username is required");
            else
            {
                if (this.Username.Length < 3)
                    errors.Add("Username must be at least 3 characters");
                if (this.Username.Length > 30)
                    errors.Add("Username cannot exceed 30 characters");
                if (this.Username.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
                    errors.Add("Username can only contain letters, numbers, and underscores");
            }

            return errors;
        }
    }
}

