static class JournalLogic
{
    public static void MapJournalEndpoints(this WebApplication app)
    {
        var journal = app.MapGroup("api/journal");
        journal.MapGet("/event", async (IEventStore store, GameState state) =>
        {
           var ev = await store.GetRandomEventAsync();
            if (ev is null) return Results.NotFound("No events available");
            state.CurrentEventId = ev.Id;
            return Results.Ok(new
            {
                ev.Description,
                choices = ev.Options.OrderBy( o => o.Slot).Select(o => new { o.Slot, o.Text, o.Description })
            });
        });
        // Player confirms their choice, and we return the result of that choice
        journal.MapPost("/choose/{slot:int}", async (int slot, IEventStore store, GameState state) =>
        {
            if (state.CurrentEventId is not int id) return Results.BadRequest("No active event.");
            var ev = await store.GetEventByIdAsync(id);
            var option = ev?.Options.FirstOrDefault(o => o.Slot == slot);
            if (option is null) return Results.BadRequest("Invalid choise.");

            state.Apply(option);
            state.EndDay();
            state.CurrentEventId = null;
            // TODO: check state.GameOver
            return Results.Ok(new { option.Result, state });

        });
    }
}