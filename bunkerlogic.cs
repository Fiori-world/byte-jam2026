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
            state.EndDay();

            var result = $"You slept. Day {state.Day} begins.";
            if (state.Food == 0) result += " There's no food left.";
            else if (state.Water == 0) result += " There's no water left.";
            return Results.Ok(new { Result = result, state });
        });

        // Hatch: scavenge once per day
        bunker.MapPost("/scavenge", (GameState state) =>
        {
            if (state.GameOver) return Results.BadRequest("Game over.");
            if (state.ScavengedToday) return Results.Conflict("You've already been outside today. Sleep first.");
            return Results.Ok(new { Result = state.Scavenge(), state });
        });
    }
}