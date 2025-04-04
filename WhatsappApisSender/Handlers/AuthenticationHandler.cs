using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatsappApisSender.Models;
using WhatsappApisSender.Services;
using WhatsappApisSender.Storage;
using WhatsappApisSender.Utils;

namespace WhatsappApisSender.Handlers
{
    public class AuthenticationHandler(IStorageManager<AppUser> storageManager, ITokenService tokenService) : IAuthenticationHandler
    {
        private readonly IStorageManager<AppUser> _storageManager = storageManager;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password)
        {
            var user = new AppUser { UserName = email, Email = email };
            var result = await storageManager.CreateUserAsync(user, password, AuthConstants.UserRole);

            return (true, string.Empty);
        }

        public async Task<(bool Success, string AccessToken, string RefreshToken, string Message)> LoginAsync(string email, string password)
        {
            var user = await _storageManager.FindByEmailAsync(email);
            if (user == null) return (false, string.Empty, string.Empty, "Invalid email.");

            var result = await _storageManager.PasswordSignInAsync(user, password);
            if (!result.Succeeded) return (false, string.Empty, string.Empty, "Invalid credentials.");

            var roles = await _storageManager.GetUserRolesAsync(user);

            string accessToken = _tokenService.GenerateToken(user, roles);
            string refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _storageManager.UpdateUserAsync(user);

            return (true, accessToken, refreshToken, string.Empty);
        }

        public async Task<(bool success, string accessToken, string error)> RefreshToken(string refreshToken)
        {
            var user = await _storageManager.FindUserByTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return (false, string.Empty, "Invalid or expired refresh token.");
            }

            var roles = await _storageManager.GetUserRolesAsync(user);

            string accessToken = _tokenService.GenerateToken(user, roles);
            return (true, accessToken, string.Empty);
        }

        public async Task<(bool success, string token, string message)> ForgotPasswordAsync(string email)
        {
            var user = await _storageManager.FindByEmailAsync(email);
            if (user == null) return (false, string.Empty, "User not found.");

            var token = await _storageManager.GeneratePasswordResetTokenAsync(user);

            return (true, token, "Use this token to reset your password.");
        }

        public async Task<(bool success, string message)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _storageManager.FindByEmailAsync(email);
            if (user == null) return (false, "Invalid email.");

            var result = await _storageManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }

            return (true, "Password has been reset successfully.");
        }
    }
}
