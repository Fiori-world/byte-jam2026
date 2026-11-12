using Microsoft.EntityFrameworkCore;
using Data.DataModels;
using Microsoft.EntityFrameworkCore.Sqlite;
using Data.DataModels.Tags;
namespace Data;

public class DbSeed
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new GameDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<GameDbContext>>());
        await context.Database.EnsureCreatedAsync();

    if (context == null || context.Events == null || context.EventOptions == null)
        {
            throw new NullReferenceException(
                "Null DbContext or DbSet!");
        }

        // Guarding if statements if Db.Context already has initialized

        if (context.Events.Any())
        {
            return;
        }
        if (context.EventOptions.Any())
        {
            return;
        }

        // Here's where you actually add events
        await context.Events.AddRangeAsync(
            new Event
            {
                EventId = Guid.NewGuid(),
                EventDescription = "Event Rain Description",
                Tags = {new EventTag("COMMON", "GOOD", "WATER")},
                Options = {1,2,3}
                
            },
            new Event
            {
                EventId = Guid.NewGuid(),
                EventDescription = "",
                Tags = {},
                Options = {}
            });

        await context.EventOptions.AddRangeAsync(
            new EventOption
            {
                
            },
            new EventOption 
            {
                
            });
        
        await context.SaveChangesAsync();
    }
        
}