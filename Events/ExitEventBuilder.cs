using IdleAdventure.Areas;
using IdleAdventure;

public class ExitEventBuilder
{
    private readonly string _discoveryText;
    private readonly List<(string transitionDescription, string targetArea, double chance)> _exits = new();
    private (string desc, string area)? _mainExit;

    public ExitEventBuilder(string discoveryText)
    {
        _discoveryText = discoveryText;
    }

    public ExitEventBuilder AddExit(string transitionDescription, string targetArea, double chance = 1.0)
    {
        _exits.Add((transitionDescription, targetArea, chance));
        return this;
    }
    
    public ExitEventBuilder MainExit(string description, string areaName)
    {
        _mainExit = (description, areaName);
        return this;
    }
    
    public AdventureEvent Build(Random rand)
    {
        return new AdventureEvent(_discoveryText, c =>
        {
            Thread.Sleep(GlobalTimer.EventTimer);
            List<string> available_areas = new();
            
            // Attempt all exits in random order
            var shuffled = _exits.OrderBy(_ => rand.Next()).ToList();
            foreach (var (desc, area, chance) in shuffled)
            {
                if (rand.NextDouble() < chance)
                {
                    ColorText.WriteLine(desc, Colors.Description);
                    available_areas.Add(area);
                }
            }

            if (available_areas.Count != 0)
            {
                c.CurrentArea = available_areas[rand.Next(available_areas.Count)];
                return;
            }

            // Fallback: main or default
            if (_mainExit != null)
            {
                var (desc, area) = _mainExit.Value;
                ColorText.WriteLine(desc, Colors.Description);
                c.CurrentArea = area;
            }
            else
            {
                ColorText.WriteLine("-- You decided to ignore it.", Colors.Ignore);
            }
        });
    }
}