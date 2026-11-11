using Data;
using Data.DataModels;
using Data.DataModels.Tags;
using Microsoft.EntityFrameworkCore;

interface IEventStore
{
    Task<Event?> GetRandomEventAsync();
    Task<Event?> GetEventAsync(Guid id);
}

class DatabaseEventStore(GameDbContext db) : IEventStore
{
    // COMMON shows up 6x as often as RARE (TODO: tune these numbers)
    static int Weight(Event e) =>
        (e.Tags.FirstOrDefault()?.Probability ?? Probability.COMMON) switch
        {
            Probability.COMMON   => 6,
            Probability.UNCOMMON => 3,
            _                    => 1
        };

    public async Task<Event?> GetRandomEventAsync()
    {
        var events = await db.Events.Include(e => e.Tags).ToListAsync();
        if (events.Count == 0) return null;

        var roll = Random.Shared.Next(events.Sum(Weight));
        foreach (var e in events)
        {
            roll -= Weight(e);
            if (roll < 0) return await GetEventAsync(e.EventId);
        }
        return null;
    }

    // Loads the event with its tags, its options, and each option's tags
    public Task<Event?> GetEventAsync(Guid id) =>
        db.Events.Include(e => e.Tags)
                 .Include(e => e.Options).ThenInclude(o => o.Tags)
                 .FirstOrDefaultAsync(e => e.EventId == id);
}
