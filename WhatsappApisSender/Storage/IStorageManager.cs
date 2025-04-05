using WhatsappApisSender.Models;
using WhatsappApisSender.Models.Authentication;

namespace WhatsappApisSender.Storage
{
    public interface IStorageManager
    {
        Task<AppUser?> FindByEmailAsync(string email);
        Task<OperationResult> CreateUserAsync(AppUser user, string password, string role);
        Task UpdateUserAsync(AppUser user);
        Task<AppUser?> FindUserByTokenAsync(string refreshToken);
        Task<AuthResult> PasswordSignInAsync(AppUser user, string password);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<OperationResult> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<List<UserContact>> FindUserContactsAsync(AppUser user);
        Task<List<UserMessage>> FindUserContactHistoryAsync(AppUser user, string contact);
        Task<List<UserMessage>> FindUserMessageHistoryAsync(AppUser user);
        Task<List<UserScheduledMessage>> FindDueScheduledMessagesAsync();
        Task<List<UserScheduledMessage>> FindUserScheduledMessageAsync(AppUser user);
        Task UpdateUserContactsAsync(AppUser user, List<UserContact> newContacts);
        void UpdateUserHistory(AppUser user, UserMessage message);
        void AddScheduledMessage(AppUser user, UserScheduledMessage message);
        Task<bool> RemovecheduledMessage(string messageId);
    }
}
