// Endpoints used by mirror.html
static class MirrorLogic
{
    public static void MapMirrorEndpoints(this WebApplication app)
    {
        var mirror = app.MapGroup("/api/mirror");

        // Brush teeth: small morale boost, once per day
        mirror.MapPost("/brush", (GameState state) =>
        {
            if (state.GameOver) return Results.BadRequest("Game over.");
            if (state.BrushedToday) return Results.Conflict("You already brushed today.");
            state.BrushedToday = true;
            state.ChangeMorale(1);
            return Results.Ok(new { Result = "No toothpaste, but it still helps a little. +1 morale.", state });
        });
    }
}