// Endpoints used by journal.html
static class JournalLogic
{
    public static void MapJournalEndpoints(this WebApplication app)
    {
        var journal = app.MapGroup("/api/journal");

        // Get the current event, or roll a new one if there isn't one
        journal.MapGet("/event", async (IEventStore store, GameState state) =>
        {
            // Refreshing the page shows the same event instead of rerolling
            var ev = state.CurrentEventId is Guid id
                ? await store.GetEventAsync(id)
                : null;
            if (ev is null && state.EventDoneToday)
                return Results.Conflict("You've already dealt with today's event. Sleep to start a new day.");
            ev ??= await store.GetRandomEventAsync();
            if (ev is null) return Results.NotFound("No events available.");

            state.CurrentEventId = ev.EventId;
            var options = await store.GetOptionsAsync(ev);

            return Results.Ok(new
            {
                Description = ev.EventDescription,
                Choices = options.Select((o, i) => new
                {
                    Slot = i + 1,                       // button 1, 2, 3...
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
            if (ev is null) return Results.BadRequest("Event not found.");

            var options = await store.GetOptionsAsync(ev);
            if (slot < 1 || slot > options.Count) return Results.BadRequest("Invalid choice.");

            // Snapshot stats so we can tell the player what changed
            var (food, water, health, morale) = (state.Food, state.Water, state.Health, state.Morale);

            options[slot - 1].ApplyTo(state);
            state.CurrentEventId = null;
            state.EventDoneToday = true;

            var changes = new List<string>();
            void Note(string name, int before, int after)
            {
                if (after != before) changes.Add($"{name} {(after > before ? "+" : "")}{after - before}");
            }
            Note("Food", food, state.Food);
            Note("Water", water, state.Water);
            Note("Health", health, state.Health);
            Note("Morale", morale, state.Morale);

            var result = changes.Count > 0 ? string.Join(", ", changes) : "Nothing changed.";
            return Results.Ok(new { Result = result, state });
        });
    }
}