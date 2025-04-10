using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappApisSender.Controllers.Schema.Status.ApiSchema;
using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema;
using WhatsappApisSender.Handlers;
using WhatsappApisSender.Utils;

namespace WhatsappApisSender.Controllers
{
    [ApiController]
    [Route("api/whatsapp/stauts")]
    [Authorize(Roles = AuthConstants.UserRole)]
    public class WhatsappStatusController(IWhatsappStatusHandlers whatsappStatusHandlers) : ControllerBase
    {
        private readonly IWhatsappStatusHandlers _whatsappStatusHandlers = whatsappStatusHandlers;

        [HttpPost("contact-vaild-number")]
        public async Task<IActionResult> ContactVaildNumber([FromBody] CheckContactNumberRequest request)
        {
            var (isSuccess, responseMessage) = await _whatsappStatusHandlers.CheckContactAsync(request.ToInternalSchema(), request.StatusCheckerToken);
            if (isSuccess)
            {
                return Ok(responseMessage);
            }

            return BadRequest(responseMessage);
        }
    }
}
