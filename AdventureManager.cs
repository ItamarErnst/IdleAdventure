using IdleAdventure.Areas;
using IdleAdventure;

public class AdventureManager
{
    private readonly Character character;
    private readonly Dictionary<string, Area> areas = new();
    private readonly Random rand = new();

    private string currentAreaName = "MeadowField";
    private bool hasShownEntranceMessage = false;

    private AdventureEvent? currentEvent = null;

    private volatile bool isPaused = false;
    private readonly object pauseLock = new();

    public AdventureManager(Character character)
    {
        this.character = character;
        foreach (var areaName in AreaRegistry.AllAreaNames)
            areas[areaName] = AreaRegistry.Get(areaName);
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
            Console.WriteLine(isPaused ? "\n⏸️  Game Paused" : "\n▶️  Game Resumed");
            Console.ResetColor();
        }
    }

    private void WaitIfPaused()
    {
        lock (pauseLock)
        {
            while (isPaused)
            {
                Monitor.Wait(pauseLock);
            }
        }
    }

    public async Task RunAsync()
    {
        while (true)
        {
            WaitIfPaused(); // 🔹 Pause support

            if (!areas.TryGetValue(currentAreaName, out var area))
            {
                ColorText.WriteLine($"Unknown area: {currentAreaName}", ConsoleColor.Red);
                break;
            }

            if (!hasShownEntranceMessage && !string.IsNullOrEmpty(area.EntranceMessage))
            {
                ColorText.WriteLine(area.EntranceMessage, ConsoleColor.Cyan);
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
    }
}
