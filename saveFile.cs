using System.Text.Json;

public static class saveFile
{
    const string FilePath = "gamestate.json";
    static readonly JsonSerializerOptions options = new() { WriteIndented = true };
    static readonly object fileLock = new();

    public static GameState Load()
    {
        try
        {
            if (File.Exists(FilePath))
                return JsonSerializer.Deserialize<GameState>(File.ReadAllText(FilePath), options) ?? new GameState();
        }
        catch (JsonException)
        {
            Console.WriteLine("gamestate.json was unreadable, starting a new game");
        }
        return new GameState();
    }

    public static void Save(GameState state)
    {
        lock (fileLock)
        {
            File.WriteAllText(FilePath + ".tmp", JsonSerializer.Serialize(state, options));
            File.Move(FilePath + ".tmp", FilePath, overwrite: true);
        }
    }
}