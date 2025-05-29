namespace IdleAdventure.Areas;

public class AreaFactory_Graveyard : IAreaFactory
{
    private readonly Random rand = Random.Shared;

    public Area Create()
    {
        var graveyard = new Area("Graveyard", "Graveyard")
        {
            EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You awaken among cracked gravestones beneath a gray, weeping sky...{Colors.Reset}"
        };

        graveyard.AddEvents(
            new WeightedEvent(CreatePathEvent(), 5),
            new WeightedEvent(CreateExitEvent(), 2),
            new WeightedEvent(CreateCombatEvent(), 3)
        );

        foreach (var special in CreateSpecialEvents())
            graveyard.AddEvents(special);

        return graveyard;
    }

    private PathEvent CreatePathEvent()
    {
        return new PathEvent(null, new[]
        {
            "A cold wind blows between leaning tombstones.",
            "You step around a freshly disturbed grave.",
            "The names on the headstones are too worn to read.",
            "A distant bell tolls once, then silence.",
            "The ground feels soft underfoot, unsettlingly so.",
            "An owl hoots from a dead tree."
        });
    }

    private AdventureEvent CreateExitEvent()
    {
        return new ExitEventBuilder("You notice worn paths among the gravestones.")
            .MainExit("You follow a trail of wilting lilies out of the cemetery.", "ForestShrine")
            .AddExit("A heavy iron gate creaks open to a winding road.", "RoyalMansion", 0.3)
            .AddExit("You glimpse a pale flicker vanishing into a crypt entrance.", "ForgottenCrypt", 0.3)
            .AddExit("Through broken stone walls, you see rolling green fields.", "MeadowField", 0.2)
            .Build(rand);
    }


    private AdventureEvent CreateCombatEvent()
    {
        return new CombatBuilder()
            .WithRareDrop(new AdventureEvent(
                "Beneath the skeletal remains, you find a tarnished silver amulet.",
                c => c.Inventory.AddItem("Grave Amulet"))
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            })
            .Build();
    }

    private IEnumerable<WeightedEvent> CreateSpecialEvents()
    {
        yield return new WeightedEvent(new AdventureEvent(
            "A ghostly whisper soothes your wounds before fading away.",
            c => c.HealHP(5))
        {
            Eligibility = c => c.CurrentHP < c.MaxHP
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "You find a rusted lantern still glowing faintly with blue fire.",
            c => c.Inventory.AddItem("Blue Lantern"))
        {
            Eligibility = _ => rand.NextDouble() < 0.25
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "You kneel at an ancient gravestone and feel a strange peace."),
        1);

        yield return new WeightedEvent(new AdventureEvent(
            "An old crypt door swings open slowly on its own."),
        1);
    }
}
