using WhatsappApisSender.Models.Authentication;

namespace WhatsappApisSender.Storage
{
    public interface IStorageManager<TUser> where TUser : class
    {
        Task<TUser?> FindByEmailAsync(string email);
        Task<OperationResult> CreateUserAsync(TUser user, string password, string role);
        Task UpdateUserAsync(TUser user);
        Task<TUser?> FindUserByTokenAsync(string refreshToken);
        Task<AuthResult> PasswordSignInAsync(TUser user, string password);
        Task<IList<string>> GetUserRolesAsync(TUser user);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<OperationResult> ResetPasswordAsync(TUser user, string token, string newPassword);
    }
}
