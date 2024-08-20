using Microsoft.EntityFrameworkCore;
using RedisDemo3.DBContext;

namespace RedisDemo3.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DbContextClass dbContext = scope.ServiceProvider.GetRequiredService<DbContextClass>();

            dbContext.Database.Migrate();
        }
    }
}
