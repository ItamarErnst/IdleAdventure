using System;
using System.Collections.Generic;
using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_ForestShrine : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var forest = new Area("ForestShrine", "Forest Shrine")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You arrive at a serene forest shrine, silent and overgrown.{Colors.Reset}"
            };

            forest.AddEvents(
                new WeightedEvent(CreatePathEvent(), 10),
                new WeightedEvent(CreateCombatEvent(), 6),
                new WeightedEvent(CreateMysticPond(), 2),
                new WeightedEvent(CreateItemEvent("You find an enchanted herb glowing beneath a tree.", "Enchanted Herb"), 2),
                new WeightedEvent(CreateRareItemEvent("You unearth a sacred gem from an old altar.", "Sacred Gem", 0.2), 1),
                new WeightedEvent(CreateRareEvent("An ancient spirit appears and blesses you with a stat increase!", c => c.Strength += 1, 0.05), 1)
            );

            return forest;
        }

        private PathEvent CreatePathEvent()
        {
            var path = new PathEvent(null, new[]
            {
                "You hear birds chirping in the distance.",
                "Sunlight peeks through thick canopies.",
                "A deer dashes past through the trees.",
                "Leaves rustle softly beneath your feet.",
                "You spot ancient carvings on a stone."
            });

            path.AddNext(CreateExitEvent(), _ => rand.NextDouble() < 0.15);
            return path;
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .OnWin(c =>
                {
                    c.GainXP(rand.Next(5, 10));
                    c.Inventory.AddGold(rand.Next(2, 3));
                })
                .WithRareDrop(new AdventureEvent("Tucked within the roots of an old tree, you find an ancient bark talisman.", c =>
                    c.Inventory.AddItem("Bark Talisman"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateMysticPond()
        {
            return new AdventureEvent(
                "You find a clear mystic pond. You feel revitalized as you drink.",
                c => c.HealFull());
        }

        private AdventureEvent CreateItemEvent(string description, string itemName)
        {
            return new AdventureEvent(description, c => c.Inventory.AddItem(itemName));
        }

        private AdventureEvent CreateRareItemEvent(string description, string itemName, double chance)
        {
            return new AdventureEvent(description, c => c.Inventory.AddItem(itemName))
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }

        private AdventureEvent CreateRareEvent(string description, Action<Character> effect, double chance)
        {
            return new AdventureEvent(description, effect)
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("You follow a worn path leading back to the meadows.")
                .AddExit("You find a muddy trail veering into a shadowy wetland.", "Swamp", 0.1)
                .AddExit("A chilly breeze flows down from a nearby peak.", "SnowyMountain", 0.1)
                .AddExit("You stumble into the Forgotten Crypt.", "ForgottenCrypt", 0.1)
                .AddExit("You see a small village through the trees.", "Village", 0.1)
                .AddExit("You find a hidden path to a dark cave.", "DarkCave", 0.1)
                .MainExit("You arrive at the edge of the MeadowField.", "MeadowField")
                .Build(rand);
        }
    }
}
