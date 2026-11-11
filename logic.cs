using Data;
using Data.DataModels;
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
    public int Health { get; set; } = 20;
    public Guid? CurrentEventId { get; set; }
    // TODO: Items list, active world events

    public bool GameOver => Health <= 0;

    public void Reset()
    {
        Day = 1;
        Food = 10;
        Water = 10;
        Health = 20;
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

            Change(tag.tagResourceGain.ToString(), +gain);
            Change(tag.tagResourceCost.ToString(), -cost);
        }
    }

    void Change(string resource, int amount)
    {
        switch (resource)
        {
            case "FOOD":            Food   = Math.Max(0, Food + amount); break;
            case "WATER":           Water  = Math.Max(0, Water + amount); break;
            case "HEALTHCONDITION": Health = Math.Clamp(Health + amount, 0, 20); break;
            // TODO: ITEM, WORLDEVENT, SPECIAL
        }
    }

    public void EndDay()
    {
        Day++;
        Food  = Math.Max(0, Food - 1);
        Water = Math.Max(0, Water - 1);
        if (Food == 0 || Water == 0) Health = Math.Max(0, Health - 2);
    }
}
