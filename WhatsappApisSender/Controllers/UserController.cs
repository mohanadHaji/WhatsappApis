using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappApisSender.Controllers.Schema.UserSchema;
using WhatsappApisSender.Handlers;
using WhatsappApisSender.Utils;

namespace WhatsappApisSender.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = AuthConstants.UserRole)]
    public class UserController(IUserHandlers userHandlers) : ControllerBase
    {
        private readonly IUserHandlers _userHandlers = userHandlers;

        [HttpPost("register-contact")]
        public async Task<IActionResult> RegisterContact([FromBody] AddPhoneSchema model)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Unable to identify user from token");
            }

            var (Success, Message) = await _userHandlers.HandelAddContactRequest(model, userEmail);
            if (!Success) return BadRequest(Message);

            return Created();
        }

        [HttpPost("cancel-scheduled-message")]
        public async Task<IActionResult> CancelScheduledMessage([FromBody] string messageId)
        {
            var (Success, Message) = await _userHandlers.HandelRemoveScheduledMessage(messageId);
            if (!Success) return BadRequest(Message);

            return Ok(Message);
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Unable to identify user from token");
            }

            var (Success, History) = await _userHandlers.HandelGetHistory(userEmail);
            if (!Success) return BadRequest("User not found");

            return Ok(History);
        }

        [HttpGet("contacts")]
        public async Task<IActionResult> Contacts()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Unable to identify user from token");
            }

            var (Success, Contacts) = await _userHandlers.HandelGetContacts(userEmail);
            if (!Success) return BadRequest("User not found");

            return Ok(Contacts.Select(contact => new { contact.Name, contact.PhoneNumber}));
        }

        [HttpGet("contact-history")]
        public async Task<IActionResult> ContactHistory([FromQuery] string contact)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Unable to identify user from token");
            }

            var (Success, Messages) = await _userHandlers.HandelGetContactHistory(userEmail, contact);
            if (!Success) return BadRequest("User not found");

            return Ok(Messages.Select(message => new { message.RequestBody, message.IsSuccessfull, message.IsScheduled }));
        }

        [HttpGet("scheduled-messages")]
        public async Task<IActionResult> Scheduled()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Unable to identify user from token");
            }

            var (Success, Messages) = await _userHandlers.HandelGetScheduledMessages(userEmail);
            if (!Success) return BadRequest("User not found");

            return Ok(Messages.Select(message => new { userEmail, message.RequestBody, message.DueDateUTC, message.Id }));
        }
    }
}
