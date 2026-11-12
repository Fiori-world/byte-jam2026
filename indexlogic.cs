// Endpoints used by the menu and setup screens (index.html, introScreen.html)
static class IndexLogic
{
    public static void MapIndexEndpoints(this WebApplication app)
    {
        var game = app.MapGroup("/api/game");

        // Prices and credits for the intro screen (defined in Shop)
        game.MapGet("/shop", () => new { Shop.Credits, Shop.Prices, Shop.RationPrice, Shop.WaterPrice });

        // Start a new game with the supplies bought on the intro screen
        game.MapPost("/new", (NewGameRequest req, GameState state) =>
        {
            if (!Shop.Credits.TryGetValue(req.Difficulty, out var budget))
                return Results.BadRequest("Unknown difficulty.");
            if (req.Food < 0 || req.Water < 0)
                return Results.BadRequest("Invalid amounts.");
            if (req.Items.Any(i => !Shop.Prices.ContainsKey(i)) || req.Items.Distinct().Count() != req.Items.Count)
                return Results.BadRequest("Invalid items.");

            // Don't trust the browser's math: check the cost here too
            if (Shop.Cost(req) > budget)
                return Results.BadRequest("Not enough credits.");

            state.Reset(req.Difficulty, req.Food, req.Water, req.Items);
            return Results.Ok(state);
        });

        // Current stats (any page can use this)
        game.MapGet("/state", (GameState state) => state);

        
    }
}