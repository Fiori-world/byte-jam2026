static class BunkerLogic
{
    public static void MapBunkerEndpoints(this WebApplication app)
    {
        var bunker = app.MapGroup("/api/bunker");
        bunker.MapPost("/sleep", (GameState state) =>
        {
           if (state.GameOver) return Results.BadRequest("Game over.");
           state.EndDay();
           return Results.Ok(state); 
        });
    }
}