using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allow the HTML frontend to call this API
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// Database connection
// builder.Services.AddDbContext<GameDbContext>(o => o.UseSqlite("Data Source=events.db"));

builder.Services.AddSingleton<IEventStore, DatabaseEventStore>();
var app = builder.Build();
app.UseCors();


// using(var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
//     db.Database.EnsureCreated();
//     // TODO: add starter events if the database is empty
// }

app.MapIndexEndpoints();

// TODO: add endpoints for the frontend to call, e.g.:

app.Run();


// Game State
class GameState
{
    // will fill in with game state stuff
    public int Day { get; set; } = 1;
    public int? CurrentEventId { get; set; }

    public bool GameOver => false;

    public void Reset()
    {
        Day = 1;
        CurrentEventId = null;
    }

    public void Apply(EventOption option)
    {
       // TODO: change stats based on option's effects
    }

    public void EndDay()
    {
        Day++;
        // TODO: use up daily food/water, apply penalties
    }
}