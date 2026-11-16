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
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                EventDescription = "Humans are social creatures. And being alone for a long time- isn't exactly great for your mental. \n So of course when I heard someone else talking I was excited to start a conversation...! Until I realized I was still alone.",
                Tags = {new EventTag("UNCOMMON", "BAD", "MORALE")},
                Options = {4,5,6}
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
                OptionId = 2,
                OptionDescription = "Open your mouth and go aaaaaaaaaaaaaaaaaaa",
                Tags = {new OptionTag("COMMON", "GOOD", "MORALE", "NONE")},
                ResultDescription = "You didn't get any water from that- but it made you feel like a kid again, and that morale will keep you going!",
                Value = [20, 0]
            },
            new EventOption 
            {
                EventId = Guid.Parse("a200b500-6027-424c-918f-81a6963f6b02"),
                OptionId = 3,
                OptionDescription = "Take a natural shower!",
                Tags = {new OptionTag("COMMON", "GOOD", "HEALTHCONDITION", "NONE")},
                ResultDescription = "You rinsed off with some rainwater... you feel squeaky clean! \n You were just standing outside though. Hope nobody was watching!",
                Value = [25, 0]
            },
            new EventOption
            {
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                OptionId = 4,
                OptionDescription = "I'm alone. Best to aknowledge that than pretend there's someone else here.",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "NONE", "MORALE")},
                ResultDescription = "You accept the solumn reality of your situation. You're alone. The world you know is gone and you just have to survive until things can go back to normal. \n ...That didn't make you feel any better- well- worse actually, but it's the truth. Better to stay grounded in something sad than delude yourself.",
                Value = [0, -20]
            },
            new EventOption 
            {
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                OptionId = 5,
                OptionDescription = "Distract myself from this nightmare by stress eating!!!",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "MORALE", "FOOD")},
                ResultDescription = "You decide to drown your sorrows in a tin can. You savor every bite, drowning out the maddening thoughts with savory tomato basil. \n That throws off your rationing plan a bit- but the voices are quiet again... for now.",
                Value = [5, -1]
            },
            new EventOption 
            {
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                OptionId = 6,
                OptionDescription = "I'm not crazy! I'm fine! Everything is fine!",
                Tags = {new OptionTag("UNCOMMON", "BAD", "MORALE", "HEALTHCONDITION")},
                ResultDescription = "You decide to tell yourself that everything is fine, actually. And that nothing about this is concerning whatsoever. Ignorance is bliss right? \n You actually manage to buy your own boldface lie, but the nerves won't go away. Your heart is racing and your head hurts. This much stress isn't good for your health, but you're still convinced that everything is JUST FINE!!!",
                Value = [10, -10]
            });

        
        await context.SaveChangesAsync();
    }
        
}