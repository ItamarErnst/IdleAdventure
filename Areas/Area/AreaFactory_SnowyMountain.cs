using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_SnowyMountain : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var mountain = new Area("SnowyMountain", "Snowy Mountain")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You begin your ascent of the snowy mountain. The air is crisp, and the silence heavy.{Colors.Reset}"
            };

            mountain.AddEvents(
                new WeightedEvent(CreatePathEvent(), 8),
                new WeightedEvent(CreateCombatEvent(), 4),
                new WeightedEvent(CreateAvalancheEvent(), 1),
                new WeightedEvent(CreateTreasureEvent("You find a weathered backpack containing supplies.", 5, 8, 0.25), 1),
                new WeightedEvent(CreateHealingEvent("You find a warm cave where you can rest and recover.", 6), 2),
                new WeightedEvent(CreateExitEvent(), 2)
            );

            return mountain;
        }

        private PathEvent CreatePathEvent()
        {
            return new PathEvent(null, new[]
            {
                "Snow crunches under your boots.",
                "A chilling wind howls past your ears.",
                "You pass a frozen stream winding down the slope.",
                "You spot claw marks in the snow near a pine tree.",
                "The sky darkens as a storm begins to form.",
                "Your breath fogs in the freezing air."
            });
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .OnWin(c =>
                {
                    c.GainXP(rand.Next(4, 8));
                    c.Inventory.AddGold(rand.Next(3, 7));
                })
                .WithRareDrop(new AdventureEvent("Buried in the snow, you find an ancient frost crystal.", c =>
                    c.Inventory.AddItem("Frost Crystal"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateAvalancheEvent()
        {
            return new AdventureEvent("An avalanche thunders down! You barely manage to escape.")
            {
                Eligibility = _ => rand.NextDouble() < 0.15
            };
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

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("The wind howls around a steep mountain pass...")
                .MainExit("You descend into the misty forest at the mountain’s base.", "ForestShrine")
                .AddExit("You slip through a narrow cave hidden behind an icy waterfall.", "DarkCave", 0.1)
                .AddExit("You follow a herd of snow elk to a field far below.", "MeadowField", 0.1)
                .Build(rand);
        }

    }
}
