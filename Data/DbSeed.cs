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
                EventId = new Guid("2d64509a-e02d-4e7e-8cb7-c09b21aa4ab"),
                EventDescription = "I was awoken from my restful slumber on the hard concrete floor by the sound of something I hadn't heard in a long time \n Is that... rain?? The thought of fresh rainwater make my mouth taste dryer. Then again, It could be dangerous out there. Maybe this is a trap..? \n Oh who am I kidding? It would take a town's worth of rattlesnakes to make that much noise- It's rain! I've GOT to catch it!",
                Tags = {new EventTag("COMMON", "GREAT", "WATER")},
                Options = {new Guid("1495c2a1-750f-44af-8ba5-ba4a494113ef"), new Guid("bf24ee34-b5c5-49d9-a483-24a2a8406413")
}
            },
            new Event
            {
                EventId = new Guid(""),
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