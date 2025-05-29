using System;
using System.Collections.Generic;
using IdleAdventure.Areas;
using IdleAdventure.Enemy;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_Cave : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var cave = new Area("DarkCave", "Dark Cave")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You step cautiously into the dark, damp cave...{Colors.Reset}"
            };

            cave.AddEvents(
                new WeightedEvent(CreatePathEvent(), 10),
                new WeightedEvent(CreateExitEvent(), 2),
                new WeightedEvent(CreateCombatEvent(), 3),
                new WeightedEvent(CreateTreasureChestEvent(), 1),
                new WeightedEvent(CreateGlowingCrystalEvent(), 2),
                new WeightedEvent(CreateRareEvent("You discover ancient cave paintings...", 0.1), 1),
                new WeightedEvent(CreateRareEvent("You stumble upon the skeleton of a past adventurer.", 0.1), 1)
            );

            return cave;
        }

        private PathEvent CreatePathEvent()
        {
            return new PathEvent(null, new[]
            {
                "You hear water dripping in the dark.",
                "A small mouse darts across the floor.",
                "You duck to avoid a low-hanging stalactite.",
                "You feel something crunch beneath your boots.",
                "A cold breeze brushes your neck.",
                "You follow a twisting cave path...",
                "Your torch flickers ominously."
            });
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .SetEnemies(AreaEnemies.caves)
                .OnWin(c => { })
                .WithRareDrop(new AdventureEvent("You uncover a treasure chest!", c => c.Inventory.AddGold(50))
                {
                    Eligibility = _ => rand.NextDouble() < 0.02
                })
                .WithRareDrop(new AdventureEvent("Behind a pile of rubble, you spot a strange gem embedded in the stone.", c =>
                    c.Inventory.AddItem("Cracked Geode"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateTreasureChestEvent()
        {
            return new AdventureEvent(
                "You find a treasure chest filled with old gold coins!",
                c => c.Inventory.AddGold(rand.Next(5, 15)))
            {
                Eligibility = _ => rand.NextDouble() < 0.25
            };
        }

        private AdventureEvent CreateGlowingCrystalEvent()
        {
            return new AdventureEvent(
                "A mysterious glowing crystal pulses with warm light.",
                c => c.HealMP(6))
            {
                Eligibility = c => c.MaxMana < c.CurrentMP
            };
        }

        private AdventureEvent CreateRareEvent(string description, double chance)
        {
            return new AdventureEvent(description)
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("You see a faint light — the cave exit!")
                .MainExit("You exit to the meadow.", "MeadowField")
                .AddExit("You step into a cold, ancient chamber...", "ForgottenCrypt", 0.2)
                .AddExit("You enter a nearby village.", "Village", 0.4)
                .AddExit("A narrow, damp tunnel leads into a rotting marshland.", "Swamp", 0.1)
                .AddExit("You hear waves crashing — a narrow tunnel leads to the shore.", "Shore", 0.4)
                .Build(rand);
        }

    }
}
