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
    public class WhatsappSenderController(IWatssappSenderHandlers watssappSenderHandlers) : ControllerBase
    {
        private readonly IWatssappSenderHandlers _watssappSenderHandlers = watssappSenderHandlers;

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
                var requestInternal = request.ToInternalSchema();
                requestInternal.To = recipient;
                var (isSuccess, responseContent) = await _watssappSenderHandlers.SendMessageAsync(requestInternal, request.AccessToken);

                results.Add(new ResponseSchema
                {
                    Recipient = recipient,
                    IsSuccess = isSuccess,
                    ResponseContent = responseContent
                });
            }

            return Accepted(results);
        }
    }
}
