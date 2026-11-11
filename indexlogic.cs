// Endpoints used by index.html
static class IndexLogic
{
    public static void MapIndexEndpoints(this WebApplication app)
    {
        var game = app.MapGroup("/api/game");

        // Start a new game
        game.MapPost("/new", (GameState state) =>
        {
            state.Reset();
            return state;
        });

        // Current stats (any page can use this)
        game.MapGet("/state", (GameState state) => state);
    }
}
