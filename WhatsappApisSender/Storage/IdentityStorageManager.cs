using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatsappApisSender.Models;
using WhatsappApisSender.Models.Authentication;

namespace WhatsappApisSender.Storage
{
    public class IdentityStorageManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IStorageManager<AppUser>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<AppUser?> FindByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<OperationResult> CreateUserAsync(AppUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return new OperationResult { Errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")), Succeeded  = result.Succeeded};
        }

        public async Task UpdateUserAsync(AppUser user)
            => await _userManager.UpdateAsync(user);

        public async Task<AppUser?> FindUserByTokenAsync(string refreshToken)
            => await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        public async Task<AuthResult> PasswordSignInAsync(AppUser user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return new AuthResult { Succeeded = result.Succeeded };
        }

        public async Task<IList<string>> GetUserRolesAsync(AppUser user)
            => await _userManager.GetRolesAsync(user);

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<OperationResult> ResetPasswordAsync(AppUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return new OperationResult { Succeeded = result.Succeeded, Errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")) };
        }
    }
}
