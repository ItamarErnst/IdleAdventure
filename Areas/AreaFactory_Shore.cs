using IdleAdventure.Enemy;

namespace IdleAdventure.Areas;

public class AreaFactory_Shore : IAreaFactory
{
    private readonly Random rand = Random.Shared;

    public Area Create()
    {
        var shore = new Area("Shore", "Shore")
        {
            EntranceMessage = $"{Colors.Bold}{Colors.NewArea}Waves crash against rocky cliffs. Salt stings your nose. You're alone by the sea.{Colors.Reset}"
        };

        shore.AddEvents(
            new WeightedEvent(CreatePathEvent(), 5),
            new WeightedEvent(CreateExitEvent(), 2),
            new WeightedEvent(CreateCombatEvent(), 3)
        );

        foreach (var special in CreateSpecialEvents())
            shore.AddEvents(special);

        return shore;
    }

    private PathEvent CreatePathEvent()
    {
        return new PathEvent(null, new[]
        {
            "You walk along a shore littered with driftwood.",
            "Seagulls cry overhead.",
            "The sand crunches under your boots.",
            "A crab scuttles into a tide pool.",
            "Foam sprays as a wave smashes against the rocks.",
            "A wrecked ship lies half-buried in the sand."
        });
    }

    private AdventureEvent CreateExitEvent()
    {
        return new ExitEventBuilder("You spot paths leading away from the coastline.")
            .MainExit("A rocky trail climbs toward inland hills.", "MeadowField")
            .AddExit("You find a narrow cave mouth hidden behind a waterfall.", "DarkCave", 0.4)
            .AddExit("An old dock leads to a foggy ferry line.", "FloatingRuins", 0.3)
            .AddExit("The sand gives way to soggy marshland.", "Swamp", 0.2)
            .Build(rand);
    }

    private AdventureEvent CreateCombatEvent()
    {
        return new CombatBuilder()
                .SetEnemies(AreaEnemies.shore)
            .WithRareDrop(new AdventureEvent(
                "You find a barnacle-covered coin etched with unknown symbols.",
                c => c.Inventory.AddItem("Ancient Coin"))
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            })
            .Build();
    }

    private IEnumerable<WeightedEvent> CreateSpecialEvents()
    {
        yield return new WeightedEvent(new AdventureEvent(
            "You rest on a sun-warmed rock, feeling your wounds ease.",
            c => c.HealHP(3))
        {
            Eligibility = c => c.CurrentHP < c.MaxHP
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "You find a waterproof pouch with 5 gold inside!",
            c => c.Inventory.AddGold(5))
        {
            Eligibility = _ => rand.NextDouble() < 0.3
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "A fish leaps from the water and vanishes."),
        1);

        yield return new WeightedEvent(new AdventureEvent(
            "You spot a ghostly silhouette walking across the waves."),
        1);
    }
}
