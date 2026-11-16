using Data;
using Data.DataModels;
using Data.DataModels.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Port: hosting services tell the app which port to use (PORT, or ASPNETCORE_URLS / _HTTP_PORTS,
// which ASP.NET reads by itself). With none of those set (running on your PC), use 5000.
var hostPort = Environment.GetEnvironmentVariable("PORT");
if (hostPort is not null)
    builder.WebHost.UseUrls($"http://0.0.0.0:{hostPort}");
else if (Environment.GetEnvironmentVariable("ASPNETCORE_URLS") is null &&
         Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS") is null)
    builder.WebHost.UseUrls("http://localhost:5000");

// Allow the HTML frontend to call this API
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Database connection (TODO: swap UseSqlite for whatever engine the DB person picks)
builder.Services.AddDbContext<GameDbContext>(o => o.UseSqlite("Data Source=events.db"));

// Where events come from (Scoped because it uses the database)
builder.Services.AddScoped<IEventStore, DatabaseEventStore>();

// Every player (device) has their own game (see gamestore.cs).
// Endpoints still just ask for "GameState" and get the game of whoever sent the request.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<GameStore>();
builder.Services.AddScoped(sp => sp.GetRequiredService<GameStore>()
    .For(GameStore.PlayerId(sp.GetRequiredService<IHttpContextAccessor>().HttpContext)));

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
   // After any game request, save that player's game
   if (context.Request.Path.StartsWithSegments("/api"))
    context.RequestServices.GetRequiredService<GameStore>().Save(GameStore.PlayerId(context));
});

// Serve the game's pages from this same server, so one address runs everything.
// Only these folders are shared: never the code, the save file or the database.
var root = app.Environment.ContentRootPath;
foreach (var folder in new[] { "html", "css", "javascript", "assets" })
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(root, folder)),
        RequestPath = "/" + folder
    });
app.MapGet("/", () => Results.File(Path.Combine(root, "index.html"), "text/html"));
app.MapGet("/index.html", () => Results.File(Path.Combine(root, "index.html"), "text/html"));

// QR code page: open <your site>/qr and scan it to play on a phone
app.MapGet("/qr", () => Results.Redirect("/html/qr.html"));

// Endpoints for each page
app.MapIndexEndpoints();     // indexlogic.cs
app.MapJournalEndpoints();   // journallogic.cs
app.MapBunkerEndpoints();    // bunkerlogic.cs
app.MapMirrorEndpoints();

app.Run();


// ---------- Game State ----------
public class GameState
{
    public int Day { get; set; } = 1;
    public int Food { get; set; } = 10;
    public int Water { get; set; } = 10;
    public const int MaxHealth = 100;
    public int Health { get; set; } = 100;
    public const int MaxMorale = 50;
    public int Morale { get; set; } = 25;
    public Guid? CurrentEventId { get; set; }
    public string Difficulty { get; set; } = "";
    public List<string> Items { get; set; } = [];
    public bool EventDoneToday { get; set; }
    public bool ScavengedToday { get; set ;}
    public bool BrushedToday { get; set; }
    public List<Guid> SeenEvents { get; set; } = [];   // events shown this cycle (no repeats until all are seen)

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
        ScavengedToday = false;
        BrushedToday = false;
        SeenEvents = [];
    }

    // The option's tags (from the database) decide how the stats change
    public void ChangeResource(string resource, int amount)
    {
        switch (resource)
        {
            case "FOOD":            Food   = Math.Max(0, Food + amount); break;
            case "WATER":           Water  = Math.Max(0, Water + amount); break;
            case "HEALTHCONDITION": Health = Math.Clamp(Health + amount, 0, MaxHealth); break;
            case "MORALE":          ChangeMorale(amount); break;
            // TODO: ITEM, WORLDEVENT, SPECIAL
        }
    }

    public static readonly Dictionary<string, string> Locations = new()
    {
        ["gasStation"] = "gas station",
        ["office"]     = "office",
        ["lake"]       = "lake"
    };

    // Go outside once a day. Without a map you end up somewhere random.
    public string Scavenge(string location)
    {
        ScavengedToday = true;
        var rng = Random.Shared;
        var result = "";

        if (!Items.Contains("Map"))
        {
            var picked = Locations.Keys.ElementAt(rng.Next(Locations.Count));
            if (picked != location) result += $"Without a map you got lost and ended up at the {Locations[picked]}. ";
            location = picked;
        }

        int food = 0, water = 0, danger;
        switch (location)
        {
            case "gasStation": food = rng.Next(1, 4); water = rng.Next(0, 2); danger = 40; break;
            case "office":     food = rng.Next(0, 2); water = rng.Next(0, 2); danger = 20; break;
            default:           water = rng.Next(2, 5);                        danger = 30; break;   // lake
        }
        if (Items.Contains("Crowbar")) { food++; water++; }

        ChangeResource("FOOD", food);
        ChangeResource("WATER", water);
        result += $"You found {food} food and {water} water.";

        if (location == "office" && !Items.Contains("Medicine") && rng.Next(100) < 30)
        {
            Items.Add("Medicine");
            result += " You also found some medicine in a desk drawer!";
        }

        danger += Difficulty switch { "Easy" => -10, "Hard" => 15, _ => 0 };
        if (Items.Contains("Radio")) danger -= 10;

        if (rng.Next(100) < danger)
        {
            int damage = rng.Next(10, 26);
            bool mask = Items.Contains("Gas Mask");
            if (mask) damage /= 2;
            ChangeResource("HEALTHCONDITION", -damage);
            ChangeMorale(-2);
            result += mask ? $" Toxic air out there, but the gas mask helped. -{damage} health."
                           : $" Something out there hurt you. -{damage} health.";
        }
        return result;
    }
    public string? UseItem(string item)
    {
        if (!Items.Contains(item)) return null;
        string result;
        switch (item)
        {
            case "First Aid Kit": ChangeResource("HEALTHCONDITION", 40); result = "You patched yourself up. +40 health."; break;
            case "Medicine": ChangeResource("HEALTHCONDITION", 20); ChangeMorale(2); result = "The medicine kicks in. +20 health, +2 morale."; break;
            default: return null;
        }
        Items.Remove(item);
        return result;
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

        ScavengedToday = false;
        BrushedToday = false;
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