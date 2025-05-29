

using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_Swamp : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var swamp = new Area("Swamp", "Swamp")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You wade into a murky swamp, the air thick and still.{Colors.Reset}"
            };

            swamp.AddEvents(
                new WeightedEvent(CreatePathEvent(), 8),
                new WeightedEvent(CreateCombatEvent(), 4),
                new WeightedEvent(CreateRareEvent("A hidden crocodile watches you from the reeds."), 1),
                new WeightedEvent(CreateHealingEvent("You sip from a medicinal spring hidden beneath moss.", 5), 2),
                new WeightedEvent(CreateTreasureEvent("You find a pouch tangled in roots — it contains gold!", 3, 7, 0.3), 1),
                new WeightedEvent(CreateExitEvent(), 2)
            );

            return swamp;
        }

        private PathEvent CreatePathEvent()
        {
            return new PathEvent(null, new[]
            {
                "You hear frogs croaking in the distance.",
                "The muck clings to your boots with each step.",
                "A mosquito buzzes annoyingly near your ear.",
                "Rotten logs and twisted roots block your way.",
                "You step carefully between bubbling bog pools.",
                "A swamp willow creaks under its own weight.",
            });
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .WithRareDrop(new AdventureEvent("In the reeds, you find a glistening swamp pearl.", c =>
                    c.Inventory.AddItem("Swamp Pearl"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateHealingEvent(string description, int hp)
        {
            return new AdventureEvent(description, c => c.HealHP(hp))
            {
                Eligibility = c => c.CurrentHP < c.MaxHP
            };
        }

        private AdventureEvent CreateTreasureEvent(string description, int min, int max, double chance)
        {
            return new AdventureEvent(description, c => c.Inventory.AddGold(rand.Next(min, max + 1)))
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }

        private AdventureEvent CreateRareEvent(string description)
        {
            return new AdventureEvent(description)
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("The swamp opens into faint trails of dry land...")
                .MainExit("You follow glowing mushrooms back into the quiet forest shrine.", "ForestShrine")
                .AddExit("A crooked wooden bridge leads to a distant village on stilts.", "Village", 0.1)
                .AddExit("A sunken stone stairway descends into a crypt half-swallowed by the bog.", "ForgottenCrypt", 0.05)
                .Build(rand);
        }

    }
}
