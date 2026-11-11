// Reads every event from the database. Nothing is stored in the code.
class DatabaseEventStore : IEventStore
{
    public Task<GameEvent?> GetRandomEventAsync()
    {
        // TODO: query the database for one random event and its options
        return Task.FromResult<GameEvent?>(null);
    }

    public Task<GameEvent?> GetEventAsync(int id)
    {
        // TODO: query the database for the event with this id and its options
        return Task.FromResult<GameEvent?>(null);
    }
}