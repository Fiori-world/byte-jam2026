using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

// Keeps a separate game for every player (device).
// Each browser makes a random id the first time (see api.js) and sends it in the
// X-Player-Id header. That player's game is saved to saves/<id>.json, so it survives restarts.
public partial class GameStore(IWebHostEnvironment env)
{
    readonly string folder = Path.Combine(env.ContentRootPath, "saves");
    readonly ConcurrentDictionary<string, GameState> games = new();
    static readonly JsonSerializerOptions options = new() { WriteIndented = true };

    // Ids are only letters, numbers and dashes, so they're always safe as file names
    [GeneratedRegex("^[A-Za-z0-9-]{8,64}$")]
    private static partial Regex ValidId();

    // Which player sent this request ("default" if there's no valid id, e.g. testing with curl)
    public static string PlayerId(HttpContext? context)
    {
        var id = context?.Request.Headers["X-Player-Id"].ToString();
        return id is not null && ValidId().IsMatch(id) ? id : "default";
    }

    // This player's game: from memory, else from their save file, else a fresh one
    public GameState For(string id) => games.GetOrAdd(id, Load);

    GameState Load(string id)
    {
        var path = PathFor(id);
        try
        {
            if (File.Exists(path))
                return JsonSerializer.Deserialize<GameState>(File.ReadAllText(path), options) ?? new GameState();
        }
        catch (JsonException)
        {
            Console.WriteLine($"Save for player {id} was unreadable, starting a new game");
        }
        return new GameState();
    }

    // Write this player's game to disk (only if they have one in memory)
    public void Save(string id)
    {
        if (!games.TryGetValue(id, out var state)) return;
        Directory.CreateDirectory(folder);
        var path = PathFor(id);
        lock (state)
        {
            File.WriteAllText(path + ".tmp", JsonSerializer.Serialize(state, options));
            File.Move(path + ".tmp", path, overwrite: true);
        }
    }

    string PathFor(string id) => Path.Combine(folder, id + ".json");
}
