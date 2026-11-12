using Data;
using Data.DataModels;
using Data.DataModels.Tags;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allow the HTML frontend to call this API
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Database connection (TODO: swap UseSqlite for whatever engine the DB person picks)
builder.Services.AddDbContext<GameDbContext>(o => o.UseSqlite("Data Source=events.db"));

// Where events come from (Scoped because it uses the database)
builder.Services.AddScoped<IEventStore, DatabaseEventStore>();

// One shared game state for every endpoint
builder.Services.AddSingleton<GameState>();

var app = builder.Build();
app.UseCors();

// Endpoints for each page
app.MapIndexEndpoints();     // indexlogic.cs
app.MapJournalEndpoints();   // journallogic.cs

app.Run();


// ---------- Game State ----------
class GameState
{
    public int Day { get; set; } = 1;
    public int Food { get; set; } = 10;
    public int Water { get; set; } = 10;
    public int Health { get; set; } = 100;
    public const int MaxMorale = 50;
    public int Morale { get; set; } = 25;
    public Guid? CurrentEventId { get; set; }
    // TODO: Items list, active world events

    public bool GameOver => Health <= 0;

    public void Reset()
    {
        Day = 1;
        Food = 10;
        Water = 10;
        Health = 100;
        Morale = 25;
        CurrentEventId = null;
    }

    // The option's tags (from the database) decide how the stats change
    public void Apply(EventOption option)
    {
        foreach (var tag in option.Tags)
        {
            // Better quality = gain more, lose less (TODO: tune these numbers)
            int gain = (int)tag.tagQuality + 1;   // HORRIBLE 1 ... INCREDIBLE 5
            int cost = 5 - (int)tag.tagQuality;   // HORRIBLE 5 ... INCREDIBLE 1
            if (Morale < 12) gain = Math.Max(0, gain - 1);

            Change(tag.tagResourceGain.ToString(), +gain);
            Change(tag.tagResourceCost.ToString(), -cost);

            Morale += tag.tagQuality switch
            {
                Tag.Quality.HORRIBLE => -8,
                Tag.Quality.BAD => -3,
                Tag.Quality.GOOD => +3,
                Tag.Quality.INCREDIBLE => +8,
                _                       => 0
            };
            Morale = Math.Clamp(Morale, 0, MaxMorale);
        };
    }

    void Change(string resource, int amount)
    {
        switch (resource)
        {
            case "FOOD":            Food   = Math.Max(0, Food + amount); break;
            case "WATER":           Water  = Math.Max(0, Water + amount); break;
            case "HEALTHCONDITION": Health = Math.Clamp(Health + amount, 0, 100); break;
            // TODO: ITEM, WORLDEVENT, SPECIAL
        }
    }

    public void EndDay()
    {
        Day++;
        Food  = Math.Max(0, Food - 1);
        Water = Math.Max(0, Water - 1);
        if (Food == 0 || Water == 0)
        { 
            Health = Math.Max(0, Health - 2);
            Morale -= 3;
        } 
        else if (Food >= 5 && Water >= 5)
        {
            Morale += 1;
        }
        Morale = Math.Clamp(Morale, 0, MaxMorale);

        if (Morale == 0) Health = Math.Max(0, Health - 1);
    }
}
