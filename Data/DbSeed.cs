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
                EventDescription = "The clouds in the sky looked gloomy today, but not with the 'radioactive fallout' kind of gray- but an actual NORMAL raincloud! Water is scarce in the parched wasteland. How will I utilize the rain before it's gone?",
                Tags = {new EventTag("COMMON", "GOOD", "WATER")},
                Options = {1,2,3}
                
            },
            
            new Event
            {
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                EventDescription = "Humans are social creatures. And being alone for a long time- isn't exactly great for your mental. \n So of course when I heard someone else talking I was excited to start a conversation...! Until I realized I was still alone.",
                Tags = {new EventTag("UNCOMMON", "BAD", "MORALE")},
                Options = {4,5,6}
            },
            new Event
            {
                EventId = Guid.Parse("696e7b70-671b-40ae-97d4-454565726dd4"),
                EventDescription = "You awaken from sleep in a cold sweat. You've never been the superstitious type, but something surreal visited you in your dreams. It's really foggy, but you're thinking that writing it out may help you remember what uncanny vison came to you in your dreams...",
                Tags = {new EventTag("RARE", "INCREDIBLE", "SPECIAL")},
                Options = {7,8,9}
            });
            /*
            new Event
            {
                EventId = Guid.Parse("e80a1fa4-9950-48e8-b0ce-ac73fd60d622"),
                EventDescription = "You woke up today to realize you- for some reason stuffed paper up your nose in your sleep. Clueless- you removed the paper- only to be hit with the most repugnant smell you've EVER had the displeasure of experiencing. Like a rotten skunk layed to rest in a pool of stagnant skum, sundried and vintage aged with bleu cheese. You HAVE to do something about this...",
                Tags = {new EventTag("UNCOMMON", "BAD", "HEALTHCONDITION")},
                Options = {10,11,12}
            },
            new Event
            {
                EventId = Guid.Parse("20b0d137-592c-4a53-b73c-0db5405f242b"),
                EventDescription = "You are stirred from a restful slumber on the concrete floor by a gentlemanly knock on your door. You cautiously peek outside only to be greeted by a strange man dressed in a bright red suit complete with red fedora, each pinstriped with sparkly maroon sequins. 'Hello good sir and or madam!' he says, salesmanly, 'Would you like to trade?'",
                Tags = {new EventTag("COMMON", "GOOD", "SPECIAL")},
                Options = {13,14,15}
            },
            new Event
            {
                EventId = Guid.Parse("07cae63f-6bd8-4e64-90c2-a2cb9bbf91b7"),
                EventDescription = "You wake up to... nothing. Not a sound from above the world. Which is strange. VERY strange. Normally the world above won't stop chatting your ear off- but now it's suddenly patient. Reserved- ready. Then- the hatch begins to move on its own. You have a bad feeling about this...",
                Tags = {new EventTag("RARE", "HORRIBLE", "SPECIAL")},
                Options = {16,17,18}
            },
            new Event
            {
                EventId = Guid.Parse("e9b19886-1338-49ef-a242-7ed1465520ff"),
                EventDescription = "Today, you wake up, and the ground (for some inexplicable reason) doesn't hurt your back. You grab your rations, and the pack is your favorite. The water somehow just got below room temperature and tastes crisp and fresh. Today is- going really good for some reason. Is today just- a good day? No catch?",
                Tags = {new EventTag("COMMON", "MORALE", "SPECIAL")},
                Options = {19,20,21}
            });
            // TODO:  INCORPORATE EVENT OPTIONS FOR THESE EVENTS

        
            new EventOption 
            {
                EventId = Guid.Parse(""),
                OptionId = 0,
                OptionDescription = "",
                Tags = {new OptionTag("", "", "", "")},
                ResultDescription = "",
                Value = []
            },
            */

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
            },
            new EventOption 
            {
                EventId = Guid.Parse("696e7b70-671b-40ae-97d4-454565726dd4"),
                OptionId = 7,
                OptionDescription = "I think I saw... My family. My friends. They're alive!",
                Tags = {new OptionTag("RARE", "INCREDIBLE", "MORALE", "NONE")},
                ResultDescription = "They are. For some reason- in your sleep you left your body and flew through the sky, witnessing it all. And in that emphemeral state you saw everyone you cared about from your old life, alive and well. Taken care of in a refugee camp. And even in the dim light of the shelter- the world suddenly seems a lot brighter.",
                Value = [50, 0]
            },
            new EventOption 
            {
                EventId = Guid.Parse("696e7b70-671b-40ae-97d4-454565726dd4"),
                OptionId = 8,
                OptionDescription = "I saw myself at home before this all. Unscarred by the wasteland.",
                Tags = {new OptionTag("RARE", "INCREDIBLE", "HEALTHCONDITION", "NONE")},
                ResultDescription = "You remember it as vividly as a chilhood memory. Bathing in the warm glow of the sun. Green grass. Fresh air. Not a cloud in sight. You deeply breathe in the morning air, and a warmth spreads through you. Your eyelids flutter, and suddenly- you're in perfect health.",
                Value = [100, 0]
            },
            new EventOption 
            {
                EventId = Guid.Parse("696e7b70-671b-40ae-97d4-454565726dd4"),
                OptionId = 9,
                OptionDescription = "I saw my bunker- stocked to the brim! Shelf full of supplies.",
                Tags = {new OptionTag("RARE", "INCREDIBLE", "FOOD", "WATER")},
                ResultDescription = "You dreamed of that security. The peace and comfort that comes with wealth. You closed your eyes tight and wished. It looks like someone actually heard your wish- actually delivered you a miracle. You woke up to find your shelf full of food and water.",
                // I'm doing a little code trickery here- instead of a negative value I'm using a positive to add supplies
                Value = [5,5]
            }
            
            );

        
        await context.SaveChangesAsync();
    }
        
}