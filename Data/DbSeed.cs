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
                EventId = Guid.Parse("a200b500-6027-424c-918f-81a6963f6b02"),
                EventDescription = "Event Rain Description",
                Tags = {new EventTag("COMMON", "GOOD", "WATER")},
                Options = {1,2,3}
                
            },
            new Event
            {
                
            });

        await context.EventOptions.AddRangeAsync(
            new EventOption
            {
                EventId = Guid.Parse("a200b500-6027-424c-918f-81a6963f6b02"),
                OptionId = 1,
                OptionDescription = "Catch some rain with some bottles!",
                Tags = {new OptionTag("COMMON", "GOOD", "WATER", "NONE")},
                ResultDescription = "You caught 2 extra bottles of water from the rain!",
                Value = [2, 0]
            },
            new EventOption 
            {
                EventId = Guid.Parse("a200b500-6027-424c-918f-81a6963f6b02"),
                OptionId = 1,
                OptionDescription = "Open your mouth and go aaaaaaaaaaaaaaaaaaa",
                Tags = {new OptionTag("COMMON", "GOOD", "MORALE", "NONE")},
                ResultDescription = "You didn't get any water from that- but it made you feel like a kid again, and that morale will keep you going!",
                Value = [20, 0]
            },
            new EventOption 
            {
                EventId = Guid.Parse("a200b500-6027-424c-918f-81a6963f6b02"),
                OptionId = 1,
                OptionDescription = "Take a natural shower!",
                Tags = {new OptionTag("COMMON", "GOOD", "HEALTHCONDITION", "NONE")},
                ResultDescription = "You rinsed off with some rainwater... you feel squeaky clean! \n You were just standing outside though. Hope nobody was watching!",
                Value = [25, 0]
            });

        
        await context.SaveChangesAsync();
    }
        
}