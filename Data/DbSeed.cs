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
            },
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
                Tags = {new EventTag("COMMON", "NEUTRAL", "SPECIAL")},
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
                EventDescription = "Today, you wake up, and the ground (for some inexplicable reason) doesn't hurt your back. You grab your rations, and the pack is your favorite. The water somehow just got below room temperature and tastes crisp and fresh. Today is- going really good for some reason, and all your regular chores are done way early. What should you do with all this extra time?",
                Tags = {new EventTag("COMMON", "MORALE", "SPECIAL")},
                Options = {19,20,21}
            });
            // TODO:  INCORPORATE EVENT OPTIONS FOR THESE EVENTS
            // Done now :3

            /*
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
            // -----------------------------------------------------------------------------
            // Event Set:  Rain
            // -----------------------------------------------------------------------------
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
            // -----------------------------------------------------------------------------
            // Event Set:  Whispers
            // -----------------------------------------------------------------------------
            new EventOption
            {
                EventId = Guid.Parse("9b676360-4672-421b-a5d3-75133b243ae0"),
                OptionId = 4,
                OptionDescription = "I'm alone. Best to aknowledge that than pretend there's someone else here.",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "NONE", "MORALE")},
                ResultDescription = "You accept the solumn reality of your situation. You're alone. The world you know is gone and you just have to survive until things can go back to normal. \n ...That didn't make you feel any better- well- worse actually, but it's the truth. Better to stay grounded in something sad than delude yourself.",
                Value = [0, -10]
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
            // -----------------------------------------------------------------------------
            // Event Set:  Miracle
            // -----------------------------------------------------------------------------
            
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
            },
            // -----------------------------------------------------------------------------
            // Event Set:  Smell
            // -----------------------------------------------------------------------------
            new EventOption 
            {
                EventId = Guid.Parse("e80a1fa4-9950-48e8-b0ce-ac73fd60d622"),
                OptionId = 10,
                OptionDescription = "Maybe one of the seals on the rations broke?",
                Tags = {new OptionTag("UNCOMMON", "BAD", "FOOD", "MORALE")},
                ResultDescription = "You brace yourself for the worst checking behind your shelf of provisions- and boy you still were not prepared. It appears some kind of mole-rat thing broke into the shelter wall smelling food- but it died after it got stuck, rotting and stinking up the place. You actually manage to steal back the can of food it stole, but disposing of a festering rat corpse is just really really gross- You have nightmares about it now.",
                Value = [1, -15]
            },
            new EventOption 
            {
                EventId = Guid.Parse("e80a1fa4-9950-48e8-b0ce-ac73fd60d622"),
                OptionId = 11,
                OptionDescription = "Put the paper back up your nose. Hold your breath until its gone.",
                Tags = {new OptionTag("UNCOMMON", "BAD", "NONE", "HEALTHCONDITION")},
                ResultDescription = "You decide to tough it out and plug up all of your olfactory senses until the odor dissapates. This however, causes not enough oxygen to get to your brain, and you eventually pass out. Thankfully you falling to the ground dislodged the rag from your nose, and the smell is already gone! But you definitely hit the concrete head first. Ouch.",
                Value = [0, -20]
            },
            new EventOption 
            {
                EventId = Guid.Parse("e80a1fa4-9950-48e8-b0ce-ac73fd60d622"),
                OptionId = 12,
                OptionDescription = "Wait- is it coming from... me?",
                Tags = {new OptionTag("UNCOMMON", "BAD", "HEALTHCONDITION", "WATER")},
                ResultDescription = "The question is practically rhetorical. It is you. And LORDIE do you reek! The smell alone will make you vomit up precious food, so in a panic you sacrifice some bottled watter to rinse off good. The smell is gone- but I hope you weren't thirsty!",
                Value = [5, -3]
            },
            // -----------------------------------------------------------------------------
            // Event Set:  Trader
            // -----------------------------------------------------------------------------
            new EventOption 
            {
                EventId = Guid.Parse("20b0d137-592c-4a53-b73c-0db5405f242b"),
                OptionId = 13,
                OptionDescription = "Accept his trade for 2 Water. (Cost: 3 Food)",
                Tags = {new OptionTag("COMMON", "NEUTRAL", "WATER", "FOOD")},
                ResultDescription = "'Fabulous!' The gentleman exclaims. He suddenly reaches inside his tux and produces 2 bottles of water and hands them over to you. You watch in fear and confusion as the gentleman walks away while eating all three rations at once. Whole. Can included.",
                Value = [2,-3]
            },
            new EventOption 
            {
                EventId = Guid.Parse("20b0d137-592c-4a53-b73c-0db5405f242b"),
                OptionId = 14,
                OptionDescription = "Accept his trade for 2 Food. (Cost: 3 Water)",
                Tags = {new OptionTag("COMMON", "NEUTRAL", "FOOD", "WATER")},
                ResultDescription = "'Fabulous!' The gentleman exclaims. He reaches into each of his pants pockets and procures a can of food from each. They're uncomfortably warm. You watch as the gentleman casually strolls away without a care in the world. A bottle of water in each hand. Each hand. All three of them.",
                Value = [2,-3]
            },
            new EventOption 
            {
                EventId = Guid.Parse("20b0d137-592c-4a53-b73c-0db5405f242b"),
                OptionId = 15,
                OptionDescription = "Tell him his deals are highway robbery and slam the hatch!!!",
                Tags = {new OptionTag("COMMON", "NEUTRAL", "MORALE", "NONE")},
                ResultDescription = "The slam of the hatch reverberates in the metal bunker. You listen closely through the hatch as the silver tounged salesman tries to coax you into taking a better deal, but you remain steadfast. The man retreats. You smirk. Even after the collapse of sociey, one rule remains true... Nobody likes door-to-door salespeople.",
                Value = [5,0]
            },
            // -----------------------------------------------------------------------------
            // Event Set:  Intruder
            // -----------------------------------------------------------------------------
            new EventOption 
            {
                EventId = Guid.Parse("07cae63f-6bd8-4e64-90c2-a2cb9bbf91b7"),
                OptionId = 0,
                OptionDescription = "HIDE. NOW.",
                Tags = {new OptionTag("RARE", "HORRIBLE", "NONE", "FOOD")},
                ResultDescription = "You cower below the large table and cover your mouth to not make a sound. The hatch slides open- and it enters. It starts as a small drizzle, but then it slops and splats onto the bunker floor. It's some kind of ooze- but it's made of meat. Bones portrude at awkward angles. Human bones. It slides over to the shelf, and it amphorphously swallows a row of cans whole. it then stretches its body and 'climbs' back out. You can't even begin to understand what just happened- other than the immediate fact that you just lost a lot of food.",
                Value = [0,-5]
            },
            new EventOption 
            {
                EventId = Guid.Parse("07cae63f-6bd8-4e64-90c2-a2cb9bbf91b7"),
                OptionId = 0,
                OptionDescription = "Prepare to fight whatever comes out- protect the supplies at all costs!",
                Tags = {new OptionTag("RARE", "HORRIBLE", "NONE", "HEALTHCONDITION")},
                ResultDescription = "You grab a particularlly hefty looking can and get ready to wallop the would-be intruder, only to find that when the hatch does open, it's one- its MANY. From the hatch, a flowing tide of rodents pours into the bunker. They scatter, all around, looking for food and water. You defiantly guard the shelf of food and begin smacking rats. But this only makes them angrier. They cover you in an instant, little rat bites all over your body- you continue to defend your supplies. The rats retreat without any of your supplies. While this may seem like a victory- you are also covered in rat bites. From dirty, radioactive, and probably diseased rats.",
                Value = [0,-40]
            },
            new EventOption 
            {
                EventId = Guid.Parse("07cae63f-6bd8-4e64-90c2-a2cb9bbf91b7"),
                OptionId = 0,
                OptionDescription =  "Thow open the hatch in the things face, then RUN!",
                Tags = {new OptionTag("RARE", "HORRIBLE", "NONE", "MORALE")},
                ResultDescription = "You throw open the hatch and bolt away- only to find... nothing. Nobody is outside. You cautiously peek back into the bunker- but there's nothing. Slowly climbing back down into the bunker- you scan the whole room. Not a trace. Except... for the mirror. Inside the mirror. Something stares back. Your reflection- the EYES of your reflection- they're not yours. Peircing. Jet black eyes. Your face goes pale. But the mirror does not reflect that. It just smiles.",
                Value = [0,-30]
            },
            // -----------------------------------------------------------------------------
            // Event Set:  Good Day
            // -----------------------------------------------------------------------------
            new EventOption 
            {
                EventId = Guid.Parse(""),
                OptionId = 0,
                OptionDescription = "Not everything has a catch. Fortune smiles upon me today, so I'm just going to enjoy it.",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "MORALE", "NONE")},
                ResultDescription = "And enjoy it you do! You kick back and relax- taking a (much needed) day off- slowing down and practicing some gratitude for what little you have here. Maybe things aren't as bad as they seem.",
                Value = [20,0]
            },
            new EventOption 
            {
                EventId = Guid.Parse(""),
                OptionId = 0,
                OptionDescription = "I'm going to double check my math on rationing. Just to be sure I'm using my supplies to their fullest.",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "FOOD", "NONE")},
                ResultDescription = "Upon reviewing your calculations... You forgot to carry the zero. You were a whole tens-place off. Wow. Yeah- you actually have a fair bit more food than you thought you did. Cool!",
                Value = [2,0]
            },
            new EventOption 
            {
                EventId = Guid.Parse(""),
                OptionId = 0,
                OptionDescription = "I still need to stay prepared. Lets use this time to check for any unadressed cuts or scrapes that might get worse without some TLC.",
                Tags = {new OptionTag("UNCOMMON", "GOOD", "HEALTHCONDITION", "NONE")},
                ResultDescription = "You spend the rest of the day patching up any little boo-boos you forgot about or just rubbed some dirt on and called it a day with ACTUAL bandages and antibiotics. You never know- could prevent you from losing a finger later on!",
                Value = [10,0]
            }
            );

        
        await context.SaveChangesAsync();
    }
        
}