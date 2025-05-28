namespace IdleAdventure.Areas;

public class AreaFactory_Meadow : IAreaFactory
{
    private readonly Random rand = Random.Shared;

    public Area Create()
    {
        var meadow = new Area("MeadowField", "Meadow Field")
        {
            EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You step into a bright meadow filled with wildflowers and buzzing insects.{Colors.Reset}"
        };

        meadow.AddEvents(
            new WeightedEvent(CreatePathEvent(), 5),
            new WeightedEvent(CreateExitEvent(), 2),
            new WeightedEvent(CreateCombatEvent(), 3)
        );

        foreach (var special in CreateSpecialEvents())
            meadow.AddEvents(special);

        return meadow;
    }

    private PathEvent CreatePathEvent()
    {
        return new PathEvent(null, new[]
        {
            "You hear birds chirping cheerfully.",
            "A butterfly flits past your face.",
            "The tall grass sways gently in the wind.",
            "You pass a patch of colorful wildflowers.",
            "You hear bees buzzing in the distance.",
            "The sun warms your back as you walk.",
            "You follow a dusty road through the meadow."
        });
    }

    private AdventureEvent CreateExitEvent()
    {
        return new ExitEventBuilder("You notice paths branching away from the meadow.")
            .MainExit("You see a small village in the distance.", "Village")
            .AddExit("You spot a dark cave entrance nearby.", "DarkCave", 0.5)
            .AddExit("You hear a whisper from the woods beyond...", "ForestShrine", 0.3)
            .AddExit("A chill runs down your spine — an overgrown crypt lies hidden in the grass.", "ForgottenCrypt", 0.05)
            .AddExit("The grass thins into cracked earth — a desert looms ahead.", "Desert", 0.1)
            .AddExit("A trail of buzzing insects leads into a reeking bog.", "Swamp", 0.1)
            .Build(rand);
    }


    private AdventureEvent CreateCombatEvent()
    {
        return new CombatBuilder()
            .OnWin(c =>
            {
                c.GainXP(rand.Next(1, 5));
                c.Inventory.AddGold(rand.Next(1, 5));
            })
            .WithRareDrop(new AdventureEvent(
                "Among the tall grass, you uncover a delicate flower glowing faintly.",
                c => c.Inventory.AddItem("Luminous Petal"))
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            })
            .Build();
    }

    private IEnumerable<WeightedEvent> CreateSpecialEvents()
    {
        yield return new WeightedEvent(new AdventureEvent(
            "A beautiful rainbow arcs across the sky. You feel renewed.",
            c => c.HealHP(6))
        {
            Eligibility = c => c.CurrentHP < c.MaxHP
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "You spot something shiny in the grass — a few dropped coins!",
            c => c.Inventory.AddGold(rand.Next(2, 6)))
        {
            Eligibility = _ => rand.NextDouble() < 0.25
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "You find an old abandoned hunter's cabin.")
        {
            Eligibility = _ => rand.NextDouble() < 0.1
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "A gentle deer watches you silently before disappearing into the woods.")
        {
            Eligibility = _ => rand.NextDouble() < 0.1
        }, 1);
    }
}
