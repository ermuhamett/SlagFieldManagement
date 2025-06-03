using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Api.Middleware;
using SlagFieldManagement.Infrastructure;

namespace SlagFieldManagement.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
