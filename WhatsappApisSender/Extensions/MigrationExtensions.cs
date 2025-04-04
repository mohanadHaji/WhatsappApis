using Microsoft.EntityFrameworkCore;
using WhatsappApisSender.Models.Database;

namespace WhatsappApisSender.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();

            using DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            context.Database.Migrate();
        }
    }
}
