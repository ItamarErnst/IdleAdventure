using IdleAdventure.Areas;
using IdleAdventure;

public class AdventureManager
{
    private readonly Character character;
    private readonly Dictionary<string, Area> areas = new();
    private readonly Random rand = new();

    private string currentAreaName = "MeadowField";
    private bool hasShownEntranceMessage = false;
    public static event Action? OnPlayerDeath;

    private volatile bool isPaused = false;
    private readonly object pauseLock = new();

    public AdventureManager(Character character)
    {
        this.character = character;

        var registry = new AreaRegistry();
        foreach (var areaName in registry.AllAreaNames)
            areas[areaName] = registry.Get(areaName);
    }
    
    public void TogglePause()
    {
        lock (pauseLock)
        {
            isPaused = !isPaused;
            if (!isPaused)
            {
                Monitor.PulseAll(pauseLock);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(isPaused ? "\n⏸️  Game Paused" : "\n▶️  Game Resumed", ConsoleColor.Yellow);
            Console.ResetColor();
        }
    }

    public async Task RunAsync()
    {
        AdventureEvent? currentEvent = null;

        while (character.CurrentHP > 0)
        {
            // ✅ Pause support
            while (isPaused)
            {
                await Task.Delay(200);
            }

            if (!areas.TryGetValue(currentAreaName, out var area))
            {
                ColorText.WriteLine($"Unknown area: {currentAreaName}", Colors.Damage);
                break;
            }

            if (!hasShownEntranceMessage && !string.IsNullOrEmpty(area.EntranceMessage))
            {
                ColorText.WriteLine(area.EntranceMessage, Colors.NewArea);
                hasShownEntranceMessage = true;
                await Task.Delay(GlobalTimer.NewAreaTimer);
            }

            var nextEvent = area.GetNextEvent(character, currentEvent, rand);

            try
            {
                nextEvent.Execute(character);
                currentEvent = nextEvent;
            }
            catch (EventBuilder.SkipEventExecution)
            {
                continue;
            }

            if (character.CurrentArea != currentAreaName)
            {
                currentAreaName = character.CurrentArea;
                hasShownEntranceMessage = false;
                currentEvent = null;
                await Task.Delay(GlobalTimer.NewAreaTimer);
                continue;
            }

            SaveData.SaveGame(character);
            await Task.Delay(GlobalTimer.EventTimer);
        }

        character.LastDeathTime = DateTime.UtcNow; // mark death time
        SaveData.SaveGame(character);              // persist it to file
        OnPlayerDeath?.Invoke();                   // notify death
    }
}
