using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema;

namespace WhatsappApisSender.Handlers
{
    public interface IWatsappSenderHandlers
    {
        Task<(bool IsSuccess, string ResponseContent)> SendMessageAsync(BaseSchema message, string token, string userEmail);
        Task<(bool IsSuccess, string ResponseContent)> SchedulMessageAsync(BaseSchema message, string token, string userEmail, DateTime dueDateUtc);
        Task SendScheduledMessageAsync(string requestBody, string token, string userEmail, string toPhoneNumber);
    }
}
