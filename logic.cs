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
builder.Services.AddSingleton(saveFile.Load());

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
 
    await DbSeed.Initialize(services);
}
app.UseCors();

app.Use(async (context, next) =>
{
   await next();
   if (context.Request.Path.StartsWithSegments("/api"))
    saveFile.Save(context.RequestServices.GetRequiredService<GameState>()); 
});

// Endpoints for each page
app.MapIndexEndpoints();     // indexlogic.cs
app.MapJournalEndpoints();   // journallogic.cs
app.MapBunkerEndpoints();    // bunkerlogic.cs

app.Run();


// ---------- Game State ----------
public class GameState
{
    public int Day { get; set; } = 1;
    public int Food { get; set; } = 10;
    public int Water { get; set; } = 10;
    public int MaxHealth = 100;
    public int Health { get; set; } = 100;
    public const int MaxMorale = 50;
    public int Morale { get; set; } = 25;
    public Guid? CurrentEventId { get; set; }
    public string Difficulty { get; set; } = "";
    public List<string> Items { get; set; } = [];
    public bool EventDoneToday { get; set; }

    public bool GameOver => Health <= 0;

    public void Reset(string difficulty, int food, int water, List<string> items)
    {
        Difficulty = difficulty;
        Day = 1;
        Food = food;
        Water = water;
        Health = MaxHealth;
        Morale = 25;
        Items = [.. items];
        CurrentEventId = null;
        EventDoneToday = false;
    }

    // The option's tags (from the database) decide how the stats change
    public void ChangeResource(string resource, int amount)
    {
        switch (resource)
        {
            case "FOOD":            Food   = Math.Max(0, Food + amount); break;
            case "WATER":           Water  = Math.Max(0, Water + amount); break;
            case "HEALTHCONDITION": Health = Math.Clamp(Health + amount, 0, MaxHealth); break;
            // TODO: ITEM, WORLDEVENT, SPECIAL
        }
    }

    public void ChangeMorale(int amount) =>
        Morale = Math.Clamp(Morale + amount, 0, MaxMorale);
    public void EndDay()
    {
        Day++;
        Food  = Math.Max(0, Food - 1);
        Water = Math.Max(0, Water - 1);
        if (Food == 0)
        { 
            Health = Math.Max(0, Health - 20);
            Morale -= 3;
        } 
        else if (Water == 0)
        {
            Health = Math.Max(0, Health -33);
            Morale -= 5;
        }
        else if (Food >= 5 && Water >= 5)
        {
            Morale += 1;
        }
        Morale = Math.Clamp(Morale, 0, MaxMorale);

        if (Morale == 0) Health = Math.Max(0, Health - 1);

        EventDoneToday = false;
        CurrentEventId = null;
    }

    
}


public static class Shop
    {
        public const int RationPrice = 1;
        public const int WaterPrice = 1;

        public static readonly Dictionary<string, int> Credits = new() { ["Easy"] = 20, ["Medium"] = 15, ["Hard"] =10};
        public static readonly Dictionary<string, int> Prices = new() { ["Radio"] = 5, ["Map"] = 3, ["Gas Mask"] = 2, ["Crowbar"] = 3, ["First Aid Kit"] = 4, ["Medicine"] = 2 };

        public static int Cost(NewGameRequest req) =>
            req.Food * RationPrice + req.Water * WaterPrice + req.Items.Sum(i => Prices[i]);
    }
    public record NewGameRequest(string Difficulty, int Food, int Water, List<string> Items);