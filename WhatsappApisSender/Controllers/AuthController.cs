using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WhatsappApisSender.Controllers.Schema.UserSchema;
using WhatsappApisSender.Handlers;

namespace WhatsappApisSender.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthenticationHandler authenticationHandler) : ControllerBase
    {
        private readonly IAuthenticationHandler _authenticationHandler = authenticationHandler;

        [HttpPost("register")] 
        public async Task<IActionResult> Register([FromBody] CreateUserRequest model)
        {
            List<string> validationErrors = model.Validate();
            if (validationErrors.Count != 0) return BadRequest(validationErrors);

            var (Success, Message) = await _authenticationHandler.RegisterAsync(model);
            if (!Success) return StatusCode(500, Message);

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Schema.UserSchema.LoginRequest model)
        {
            var (Success, AccessToken, RefreshToken, Message) = await _authenticationHandler.LoginAsync(model.Email, model.Password);
            if (!Success) return Unauthorized(Message);

            return Ok(new { AccessToken, RefreshToken });
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest model)
        {
            var (Success, AccessToken, Message) = await _authenticationHandler.RefreshToken(model.RefreshToken);
            if (!Success) return Unauthorized(Message);

            return Ok(new { AccessToken });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var (success, token, message) = await _authenticationHandler.ForgotPasswordAsync(model.Email);
            if (!success) return BadRequest(message);

            return Ok(new { token, message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var (success, message) = await _authenticationHandler.ResetPasswordAsync(model.Email, model.ResetCode, model.NewPassword);
            if (!success) return BadRequest(message);

            return Ok(message);
        }
    }
}
