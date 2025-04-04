﻿using Microsoft.AspNetCore.Identity;

namespace WhatsappApisSender.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public List<UserContact>? UserContacts { get; set; } = [];

        public List<UserMessage>? UserMessagesHistory { get; set; } = [];

        public List<UserScheduledMessage>? UserScheduledMessages { get; set; } = [];
    }
}
