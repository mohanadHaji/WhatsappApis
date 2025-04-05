using WhatsappApisSender.Controllers.Schema.UserSchema;
using WhatsappApisSender.Models;
using WhatsappApisSender.Storage;

namespace WhatsappApisSender.Handlers
{
    public class UserHandlers(IStorageManager storageManager) : IUserHandlers
    {
        private readonly IStorageManager _storageManager = storageManager;

        public async Task<(bool Success, string Message)> HandelAddContactRequest(AddPhoneSchema request, string userEmail)
        {
            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (false, "User email is incorect");
            }

            var contacts = request.Contacts.Select(contact => new UserContact() { Name = contact.Name, PhoneNumber = contact.PhoneNumber }).ToList();
            await _storageManager.UpdateUserContactsAsync(user, contacts);
            
            return (true, string.Empty);
        }

        public async Task<(bool Success, List<UserMessage> History)> HandelGetHistory(string userEmail)
        {
            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (false, new());
            }

            var messages = (await _storageManager
                .FindUserMessageHistoryAsync(user))
                .Select(message => new UserMessage() { Id = message.Id, IsScheduled = message.IsScheduled, IsSuccessfull = message.IsSuccessfull, PhoneNumber = message.PhoneNumber, RequestBody = message.RequestBody })
                .ToList();
            return (true, messages);
        }

        public async Task<(bool Success, List<UserContact> Contacts)> HandelGetContacts(string userEmail)
        {
            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (false, new());
            }

            return (true, await _storageManager.FindUserContactsAsync(user));
        }

        public async Task<(bool Success, List<UserMessage> Messages)> HandelGetContactHistory(string userEmail, string contact)
        {
            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (false, new());
            }

            return (true, await _storageManager.FindUserContactHistoryAsync(user, contact));
        }

        public async Task<(bool Success, List<UserScheduledMessage> Messages)> HandelGetScheduledMessages(string userEmail)
        {
            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return (false, new());
            }

            return (true, await _storageManager.FindUserScheduledMessageAsync(user));
        }

        public async Task<(bool Success, string Message)> HandelRemoveScheduledMessage(string messageId)
        {
            if (await _storageManager.RemovecheduledMessage(messageId)) return (true, "Removed message");
            return (false, "Message not found");
        }
    }
}
