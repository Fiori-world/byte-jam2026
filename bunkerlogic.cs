// Endpoints used by bunker.html
static class BunkerLogic
{
    public static void MapBunkerEndpoints(this WebApplication app)
    {
        var bunker = app.MapGroup("/api/bunker");

        // Hammock: next day, eat/drink, reset daily actions
        bunker.MapPost("/sleep", (GameState state) =>
        {
            if (state.GameOver) return Results.BadRequest("Game over.");
            // You can't end the day until you've dealt with today's journal entry
            if (!state.EventDoneToday)
                return Results.Conflict("You can't sleep yet. Check the journal and deal with today's event first.");
            state.EndDay();

            var result = $"You slept. Day {state.Day} begins.";
            if (state.Food == 0) result += " There's no food left.";
            else if (state.Water == 0) result += " There's no water left.";
            return Results.Ok(new { Result = result, state });
        });

        // Hatch: scavenge once per day
        bunker.MapPost("/scavenge/{location}", (string location, GameState state) =>
        {
            if (state.GameOver) return Results.BadRequest("Game over.");
            if (!GameState.Locations.ContainsKey(location)) return Results.BadRequest("Unknown location.");
            if (state.ScavengedToday) return Results.Conflict("You've already been outside today. Sleep first.");
            var (wentTo, result) = state.Scavenge(location);
            return Results.Ok(new { Result = result, Location = wentTo, state });   // Location = where you really ended up
        });
    }
}