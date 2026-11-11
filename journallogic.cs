// Endpoints used by journal.html
static class JournalLogic
{
    public static void MapJournalEndpoints(this WebApplication app)
    {
        var journal = app.MapGroup("/api/journal");

        // Get a random event
        journal.MapGet("/event", async (IEventStore store, GameState state) =>
        {
            var ev = await store.GetRandomEventAsync();
            if (ev is null) return Results.NotFound("No events available.");

            state.CurrentEventId = ev.EventId;
            return Results.Ok(new
            {
                Description = ev.EventDescription,
                Choices = ev.Options.OrderBy(o => o.OptionId).Select(o => new
                {
                    Slot = o.OptionId,
                    Text = o.OptionDescription,         // TODO: separate button label?
                    Description = o.OptionDescription
                })
            });
        });

        // Player confirms their choice, and we return the result of that choice
        journal.MapPost("/choose/{slot:int}", async (int slot, IEventStore store, GameState state) =>
        {
            if (state.CurrentEventId is not Guid id) return Results.BadRequest("No active event.");

            var ev = await store.GetEventAsync(id);
            var option = ev?.Options.FirstOrDefault(o => o.OptionId == slot);
            if (option is null) return Results.BadRequest("Invalid choice.");

            state.Apply(option);
            state.EndDay();
            state.CurrentEventId = null;

            return Results.Ok(new { Result = "TODO: no result text in the database yet", state });
        });
    }
}
