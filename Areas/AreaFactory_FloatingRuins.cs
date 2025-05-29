using System;
using System.Collections.Generic;
using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_FloatingRuins : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var ruins = new Area("FloatingRuins", "Floating Ruins")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You step onto ancient ruins suspended in the sky. The air hums with arcane power.{Colors.Reset}"
            };

            ruins.AddEvents(
                new WeightedEvent(CreatePathEvent(), 8),
                new WeightedEvent(CreateCombatEvent(), 5),
                new WeightedEvent(CreateMysticPuzzleEvent(), 1),
                new WeightedEvent(CreateTreasureEvent("You find a glowing relic fragment.", "Relic Fragment", 0.15), 1),
                new WeightedEvent(CreateHealingEvent("A radiant aura surrounds you, mending your wounds and clearing your mind.", 5), 2),
                new WeightedEvent(CreateExitEvent(), 2)
            );

            return ruins;
        }

        private PathEvent CreatePathEvent()
        {
            return new PathEvent(null, new[]
            {
                "Stone bridges float silently over bottomless skies.",
                "Faint whispers echo from glowing glyphs.",
                "You pass a shattered statue of an unknown god.",
                "Magic energy arcs between ruined pillars.",
                "You feel gravity loosen its grip with every step.",
                "Wind whistles through levitating platforms."
            });
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .WithRareDrop(new AdventureEvent("You find a floating orb pulsing with ancient energy.", c =>
                    c.Inventory.AddItem("Arcane Orb"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateMysticPuzzleEvent()
        {
            var solved = new AdventureEvent("You solve the ancient puzzle and feel a surge of insight.", c => c.Intelligence += 1);
            var failed = new AdventureEvent("You fail to solve the riddle and feel time slip away...");

            return EventBuilder
                .Describe("You approach a glowing rune circle inscribed with a celestial riddle.")
                .WithChanceOutcome(0.4, new[] { solved }, new[] { failed })
                .Build();
        }

        private AdventureEvent CreateHealingEvent(string description, int hp)
        {
            return new AdventureEvent(description, c => c.HealHP(hp))
            {
                Eligibility = c => c.CurrentHP < c.MaxHP
            };
        }

        private AdventureEvent CreateTreasureEvent(string description, string itemName, double chance)
        {
            return new AdventureEvent(description, c => c.Inventory.AddItem(itemName))
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("Floating stairways stretch out across the sky...")
                .MainExit("You descend through a collapsing rune portal to the forest shrine.", "ForestShrine")
                .AddExit("A glowing column of light plunges straight into the crypt below.", "ForgottenCrypt", 0.1)
                .AddExit("You leap to a sky-anchored balloon that floats toward a village skyline.", "Village", 0.1)
                .Build(rand);
        }

    }
}
