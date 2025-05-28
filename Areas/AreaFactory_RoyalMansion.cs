namespace IdleAdventure.Areas;

public class AreaFactory_RoyalMansion : IAreaFactory
{
    private readonly Random rand = Random.Shared;

    public Area Create()
    {
        var mansion = new Area("RoyalMansion", "Royal Mansion")
        {
            EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You stand in a grand hall. Dust dances in sunbeams cutting through broken windows.{Colors.Reset}"
        };

        mansion.AddEvents(
            new WeightedEvent(CreatePathEvent(), 5),
            new WeightedEvent(CreateExitEvent(), 2),
            new WeightedEvent(CreateCombatEvent(), 3)
        );

        foreach (var special in CreateSpecialEvents())
            mansion.AddEvents(special);

        return mansion;
    }

    private PathEvent CreatePathEvent()
    {
        return new PathEvent(null, new[]
        {
            "A portrait of a forgotten king watches you.",
            "Velvet curtains hang shredded from tall windows.",
            "You step carefully over shattered tiles.",
            "A harp string plucks itself in the empty music room.",
            "Mice scatter under a silver serving tray.",
            "You hear faint music echoing from nowhere."
        });
    }

    private AdventureEvent CreateExitEvent()
    {
        return new ExitEventBuilder("You find a crumbling stairwell and broken doors.")
            .MainExit("You step out into the overgrown royal garden.", "Village")
            .AddExit("A narrow servant passage leads toward a shadowy chapel.", "ForgottenCrypt", 0.4)
            .AddExit("A marble hallway opens to the stables and beyond.", "ForestShrine", 0.3)
            .AddExit("A secret passage winds toward a cold cave wall.", "DarkCave", 0.2)
            .Build(rand);
    }


    private AdventureEvent CreateCombatEvent()
    {
        return new CombatBuilder()
            .OnWin(c =>
            {
                c.GainXP(rand.Next(1, 4));
                c.Inventory.AddGold(rand.Next(3, 6));
            })
            .WithRareDrop(new AdventureEvent(
                "You pry open a drawer and find a Royal Insignia ring.",
                c => c.Inventory.AddItem("Royal Insignia"))
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            })
            .Build();
    }

    private IEnumerable<WeightedEvent> CreateSpecialEvents()
    {
        yield return new WeightedEvent(new AdventureEvent(
            "You find a hidden wine bottle. It's still good. You feel refreshed.",
            c => c.HealHP(4))
        {
            Eligibility = c => c.CurrentHP < c.MaxHP
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "A jeweled hairpin lies forgotten on a dusty vanity.",
            c => c.Inventory.AddItem("Jeweled Hairpin"))
        {
            Eligibility = _ => rand.NextDouble() < 0.2
        }, 1);

        yield return new WeightedEvent(new AdventureEvent(
            "A suit of armor crashes loudly to the ground."),
        1);

        yield return new WeightedEvent(new AdventureEvent(
            "A diary page flutters past your feet, speaking of betrayal."),
        1);
    }
}
