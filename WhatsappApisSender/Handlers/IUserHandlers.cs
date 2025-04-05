using WhatsappApisSender.Controllers.Schema.UserSchema;
using WhatsappApisSender.Models;

namespace WhatsappApisSender.Handlers
{
    public interface IUserHandlers
    {
        Task<(bool Success, string Message)> HandelAddContactRequest(AddPhoneSchema request, string userEmail);
        Task<(bool Success, List<UserMessage> History)> HandelGetHistory(string userEmail);
        Task<(bool Success, List<UserContact> Contacts)> HandelGetContacts(string userEmail);
        Task<(bool Success, List<UserMessage> Messages)> HandelGetContactHistory(string userEmail, string contact);
        Task<(bool Success, List<UserScheduledMessage> Messages)> HandelGetScheduledMessages(string userEmail);
        Task<(bool Success, string Message)> HandelRemoveScheduledMessage(string messageId);
    }
}
