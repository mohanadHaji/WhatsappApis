using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema;

namespace WhatsappApisSender.Handlers
{
    public interface IWatssappSenderHandlers
    {
        Task<(bool IsSuccess, string ResponseContent)> SendMessageAsync(BaseSchema message, string token);
    }
}
