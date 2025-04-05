using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WhatsappApisSender.Models.Database
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserMessage> UserMessagesHistory { get; set; }
        public DbSet<UserScheduledMessage> UserScheduledMessages { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");
            builder.Entity<AppUser>().Property(u => u.Email).HasMaxLength(30);

            builder.Entity<UserContact>(entity =>
            {
                entity.HasKey(uc => uc.Id);
                entity.HasOne(uc => uc.AppUser)
                    .WithMany(appUser => appUser.UserContacts)
                    .HasForeignKey(uc => uc.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            builder.Entity<UserMessage>(entity =>
            {
                entity.HasKey(uc => uc.Id);

                entity.HasOne(uc => uc.AppUser)
                    .WithMany(appUser => appUser.UserMessagesHistory)
                    .HasForeignKey(uc => uc.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            builder.Entity<UserScheduledMessage>(entity =>
            {
                entity.HasKey(userScheduledMessagesUserScheduledMessage => userScheduledMessagesUserScheduledMessage.Id);
                entity.HasOne(userScheduledMessagesUserScheduledMessage => userScheduledMessagesUserScheduledMessage.AppUser)
                    .WithMany(appUser => appUser.UserScheduledMessages)
                    .HasForeignKey(userScheduledMessagesUserScheduledMessage => userScheduledMessagesUserScheduledMessage.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
        }
    }
}
