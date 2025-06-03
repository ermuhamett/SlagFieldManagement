using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Infrastructure;

namespace SlagFieldManagement.Api.Extensions;

public static class TruncateDataExtensions
{
    public static void TruncateData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        ClearStateEventStore(dbContext);
        ClearState(dbContext);
    }
    
    // Альтернатива с прямым SQL-запросом
    private static void ClearStateEventStore(ApplicationDbContext dbContext)
    {
        var events = dbContext.SlagFieldStateEvents.ToList();
        dbContext.SlagFieldStateEvents.RemoveRange(events);
        dbContext.SaveChanges();
    }

    private static void ClearState(ApplicationDbContext dbContext)
    {
        var states = dbContext.SlagFieldStates.ToList();
        dbContext.SlagFieldStates.RemoveRange(states);
        dbContext.SaveChanges();
    }
}