using IdleAdventure.Areas;
using IdleAdventure.Enemy;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_Desert : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var desert = new Area("Desert", "Desert")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You step into a vast, sun-scorched desert. The dunes stretch endlessly around you.{Colors.Reset}"
            };

            desert.AddEvents(
                new WeightedEvent(CreatePathEvent(), 8),
                new WeightedEvent(CreateCombatEvent(), 4),
                new WeightedEvent(CreateMirageEvent(), 1),
                new WeightedEvent(CreateTreasureEvent("You find a buried satchel filled with coins.", 4, 10, 0.2), 1),
                new WeightedEvent(CreateHealingEvent("You find a hidden oasis and drink deeply from its clear spring.", 7), 2),
                new WeightedEvent(CreateExitEvent(), 2)
            );

            return desert;
        }

        private PathEvent CreatePathEvent()
        {
            return new PathEvent(null, new[]
            {
                "The sand burns beneath your feet.",
                "You spot a vulture circling above.",
                "Wind howls between distant dunes.",
                "You pass a shattered stone obelisk.",
                "You squint at something shimmering on the horizon.",
                "A caravan's tracks vanish into the dust.",
            });
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .SetEnemies(AreaEnemies.desert)
                .WithRareDrop(new AdventureEvent("In the sand, you uncover a golden scorpion idol.", c =>
                    c.Inventory.AddItem("Scorpion Idol"))
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

        private AdventureEvent CreateMirageEvent()
        {
            return new AdventureEvent("You chase a mirage, only to find your path unchanged.")
            {
                Eligibility = _ => rand.NextDouble() < 0.2
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("You crest a tall dune and spy distant shapes...")
                .MainExit("A cracked trail winds down into a grassy field.", "MeadowField")
                .AddExit("You descend into a chasm that leads into a dark cave network.", "DarkCave", 0.1)
                .AddExit("A caravan route curves toward a sunbaked village on the horizon.", "Village", 0.1)
                .Build(rand);
        }

    }
}
