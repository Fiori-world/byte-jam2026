static class IndexLogic
{
    public static void MapIndexEndpoints(this WebApplication app)
    {
        var game = app.MapGroup("api/game");
        game.MapPost("/new", (GameState state) =>
        {
            // TODO: reset the stats to starting values
            return state;
        });
    }
}