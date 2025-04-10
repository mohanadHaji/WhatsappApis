using WhatsappApisSender.Controllers.Schema.Status.InternalSchema;

namespace WhatsappApisSender.Handlers
{
    public interface IWhatsappStatusHandlers
    {
        Task<(bool IsSuccess, string ResponseContent)> CheckContactAsync(CheckContactNumberRequest requestMessage, string token);
    }
}
