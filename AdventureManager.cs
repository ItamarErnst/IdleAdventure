using IdleAdventure.Areas;

namespace IdleAdventure
{
    public class AdventureManager
    {
        private readonly Character character;
        private readonly Dictionary<string, Area> areas = new();
        private readonly Random rand = new();
        private bool running = true;
        private TaskCompletionSource<bool>? resumeTcs;

        private string currentAreaName = "MeadowField";
        private bool hasShownEntranceMessage = false;

        public AdventureManager(Character character)
        {
            this.character = character;

            // Preload all known areas from the registry
            foreach (var areaName in AreaRegistry.AllAreaNames)
            {
                areas[areaName] = AreaRegistry.Get(areaName);
            }
        }

        public async Task RunAsync()
        {
            while (true)
            {
                if (!running)
                {
                    resumeTcs = new TaskCompletionSource<bool>();
                    await resumeTcs.Task;
                    hasShownEntranceMessage = false;
                }

                while (running)
                {
                    if (!areas.TryGetValue(currentAreaName, out var area))
                    {
                        ColorText.WriteLine($"Unknown area: {currentAreaName}", ConsoleColor.Red);
                        Pause(); break;
                    }

                    if (!hasShownEntranceMessage && !string.IsNullOrEmpty(area.EntranceMessage))
                    {
                        ColorText.WriteLine(area.EntranceMessage, ConsoleColor.Cyan);
                        hasShownEntranceMessage = true;
                        await Task.Delay(GlobalTimer.NewAreaTimer);
                    }

                    var currentEvent = area.GetRandomEvent(rand);
                    try
                    {
                        currentEvent.Execute(character);
                    }
                    catch (EventBuilder.SkipEventExecution)
                    {
                        continue;
                    }

                    if (character.CurrentArea != currentAreaName)
                    {
                        currentAreaName = character.CurrentArea;
                        hasShownEntranceMessage = false;
                        await Task.Delay(GlobalTimer.NewAreaTimer);
                        continue;
                    }

                    // Follow-up event
                    var next = currentEvent.GetRandomFollowUp(rand);
                    if (next != currentEvent)
                    {
                        await Task.Delay(GlobalTimer.EventTimer);
                        try
                        {
                            next.Execute(character);
                        }
                        catch (EventBuilder.SkipEventExecution)
                        {
                            continue;
                        }

                        if (character.CurrentArea != currentAreaName)
                        {
                            currentAreaName = character.CurrentArea;
                            hasShownEntranceMessage = false;
                            await Task.Delay(GlobalTimer.NewAreaTimer);
                            continue;
                        }
                    }

                    SaveData.SaveGame(character);
                    await Task.Delay(GlobalTimer.EventTimer);
                }
            }
        }

        public void Pause() => running = false;

        public void Resume()
        {
            if (!running)
            {
                running = true;
                resumeTcs?.TrySetResult(true);
            }
        }
    }
}
