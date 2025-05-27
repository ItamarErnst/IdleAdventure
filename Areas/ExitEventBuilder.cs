using IdleAdventure.Areas;
using IdleAdventure;

public class ExitEventBuilder
{
    private readonly string _discoveryText;
    private readonly List<(string transitionDescription, string targetArea, double chance)> _exits = new();

    public ExitEventBuilder(string discoveryText)
    {
        _discoveryText = discoveryText;
    }

    public ExitEventBuilder AddExit(string transitionDescription, string targetArea, double chance = 1.0)
    {
        _exits.Add((transitionDescription, targetArea, chance));
        return this;
    }

    public AdventureEvent Build(Random rand)
    {
        var discovery = new AdventureEvent(
            _discoveryText,
            c => ColorText.WriteLine("You decide to walk toward it...", ConsoleColor.Gray)
        );

        bool added = false;

        foreach (var (desc, area, chance) in _exits)
        {
            var transition = EventBuilder
                .Describe(desc)
                .WithTransition(area, 1)
                .Build();

            discovery.AddNext(transition, _ =>
            {
                bool success = rand.NextDouble() < chance;
                if (success) added = true;
                return success;
            });
        }

        // Add fallback if nothing was selected
        discovery.AddNext(
            new AdventureEvent("You wander a bit, but find nothing of interest."),
            _ => !added
        );

        return discovery;
    }
}