using Microsoft.AspNetCore.Identity;
using WhatsappApisSender.Models.Database;
using WhatsappApisSender.Models;
using Microsoft.EntityFrameworkCore;
using WhatsappApisSender.Utils;

namespace WhatsappApisSender.Extensions
{
    public static class IdentityCoreExtensions
    {
        public static void AddIdentityCoreWithPostgreSQL(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
        }

        public static async Task InitRolesAsync(this WebApplication app)
        {
            string[] roleNames = { AuthConstants.UserRole };

            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
