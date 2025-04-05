using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema;
using WhatsappApisSender.Handlers;
using WhatsappApisSender.Utils;

namespace WhatsappApisSender.Controllers
{
    [ApiController]
    [Route("api/whatsapp/sender")]
    [Authorize(Roles = AuthConstants.UserRole)]
    public class WhatsappSenderController(IWatsappSenderHandlers watssappSenderHandlers) : ControllerBase
    {
        private readonly IWatsappSenderHandlers _watssappSenderHandlers = watssappSenderHandlers;

        [HttpPost("template")]
        public async Task<IActionResult> Template([FromBody] TemplateRequest request)
        {
            return await HandleRequest(request);
        }

        [HttpPost("text")]
        public async Task<IActionResult> Text([FromBody] TextRequest request)
        {
            return await HandleRequest(request);
        }

        [HttpPost("Image")]
        public async Task<IActionResult> Image([FromBody] ImageRequest request)
        {
            return await HandleRequest(request);
        }

        [HttpPost("document")]
        public async Task<IActionResult> Document([FromBody] DocumentRequest request)
        {
            return await HandleRequest(request);
        }

        private async Task<ActionResult> HandleRequest(BaseSchema request)
        {
            var results = new List<ResponseSchema>();

            var validationErrors = request.Validate();
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }

            foreach (var recipient in request.To)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest("Unable to identify user from token");
                }

                var requestInternal = request.ToInternalSchema();
                requestInternal.To = recipient;

                var (isSuccess, responseContent) = request.ScheduledTimeInUtc == null 
                    ? await _watssappSenderHandlers.SendMessageAsync(requestInternal, request.AccessToken, userEmail)
                    : await _watssappSenderHandlers.SchedulMessageAsync(requestInternal, request.AccessToken, userEmail, request.ScheduledTimeInUtc.Value);

                results.Add(new ResponseSchema
                {
                    Recipient = recipient,
                    IsSuccess = isSuccess,
                    ResponseContent = responseContent
                });

                await Task.Delay(request.DelayBetweenMessagesInMs);
            }

            return Accepted(results);
        }
    }
}
