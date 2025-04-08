using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatsappApisSender.Models;
using WhatsappApisSender.Models.Authentication;
using WhatsappApisSender.Models.Database;

namespace WhatsappApisSender.Storage
{
    public class StorageManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DatabaseContext context) : IStorageManager
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly DatabaseContext _context = context;

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

        public async Task<List<UserContact>> FindUserContactsAsync(AppUser user)
        {
            return await _context.UserContacts
                    .Where(uc => uc.AppUserId == user.Id)
                    .ToListAsync();
        }

        public async Task<List<UserMessage>> FindUserContactHistoryAsync(AppUser user, string contact)
        {
            return await _context.UserMessagesHistory
                    .Where(uc => uc.AppUserId == user.Id && uc.PhoneNumber == contact)
                    .ToListAsync();
        }

        public async Task<List<UserMessage>> FindUserMessageHistoryAsync(AppUser user)
        {
            return await _context.UserMessagesHistory
                    .Where(uc => uc.AppUserId == user.Id)
                    .ToListAsync();
        }

        public async Task<List<UserScheduledMessage>> FindUserScheduledMessageAsync(AppUser user)
        {
            return await _context.UserScheduledMessages
                    .Where(uc => uc.AppUserId == user.Id)
                    .ToListAsync();
        }

        public async Task<List<UserScheduledMessage>> FindDueScheduledMessagesAsync()
        {
            var currentUtcTime = DateTime.UtcNow;

            return await _context.UserScheduledMessages
                .Where(msg => msg.DueDateUTC <= currentUtcTime)
                .Include(msg => msg.AppUser)
                .ToListAsync();
        }

        public async Task UpdateUserContactsAsync(AppUser user, List<UserContact> newContacts)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingContacts = await this.FindUserContactsAsync(user);

                var contactsToRemove = existingContacts
                    .Where(ec => !newContacts.Any(nc =>
                        nc.PhoneNumber == ec.PhoneNumber &&
                        nc.Name == ec.Name))
                    .ToList();

                var contactsToAdd = newContacts
                    .Where(nc => !existingContacts.Any(ec =>
                        ec.PhoneNumber == nc.PhoneNumber &&
                        ec.Name == nc.Name))
                    .ToList();

                _context.UserContacts.RemoveRange(contactsToRemove);

                foreach (var contact in contactsToAdd)
                {
                    contact.AppUserId = user.Id;
                    contact.AppUser = user;
                    _context.UserContacts.Add(contact);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void UpdateUserHistory(AppUser user, UserMessage message)
        {
            message.AppUserId = user.Id;
            message.AppUser = user;
            _context.UserMessagesHistory.Add(message);
            _context.SaveChanges();
        }

        public void AddScheduledMessage(AppUser user, UserScheduledMessage message)
        {
            message.AppUserId = user.Id;
            message.AppUser = user;
            _context.UserScheduledMessages.Add(message);
            _context.SaveChanges();
        }

        public async Task<bool> RemovecheduledMessage(string messageId)
        {
            var message = await _context.UserScheduledMessages.FirstOrDefaultAsync(m => m.Id.ToString() == messageId);

            if (message == null)
            {
                return false;
            }

            _context.UserScheduledMessages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

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

        public async Task<bool> UpdateScheduledMessage(string messageId, DateTime newDueDateUTC)
        {
            var message = await _context.UserScheduledMessages.FirstOrDefaultAsync(m => m.Id.ToString() == messageId);

            if (message == null)
            {
                return false;
            }

            message.DueDateUTC = newDueDateUTC;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
