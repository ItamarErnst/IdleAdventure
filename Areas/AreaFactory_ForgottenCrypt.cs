using System;
using System.Collections.Generic;
using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_ForgottenCrypt : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var crypt = new Area("ForgottenCrypt", "Forgotten Crypt")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You descend into a forgotten crypt, the air stale with death.{Colors.Reset}"
            };

            crypt.AddEvents(
                new WeightedEvent(CreatePathEvent(), 10),
                new WeightedEvent(CreateCombatEvent(), 6),
                new WeightedEvent(CreateDarkAltarEvent(), 2),
                new WeightedEvent(CreateRareEvent("Among the bones, you discover a mysterious charm.", c => c.Inventory.AddItem("Bone Charm"), 0.15), 1),
                new WeightedEvent(CreateRareEvent("You find a cursed relic that pulses with dark energy.", c => c.Inventory.AddItem("Cursed Relic"), 0.05), 1)
            );

            return crypt;
        }

        private PathEvent CreatePathEvent()
        {
            var path = new PathEvent(null, new[]
            {
                "Your footsteps echo ominously down the stone corridor.",
                "Dust and cobwebs coat every surface.",
                "You hear a distant clatter of bone.",
                "You light a rusted wall torch to illuminate the way.",
                "Moss-covered tombs line the narrow path."
            });

            path.AddNext(CreateExitEvent(), _ => rand.NextDouble() < 0.1);
            return path;
        }

        private AdventureEvent CreateCombatEvent()
        {
            return new CombatBuilder()
                .WithRareDrop(new AdventureEvent("You find a rare obsidian pendant hidden in the bone pile.", c =>
                    c.Inventory.AddItem("Obsidian Pendant"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
        }

        private AdventureEvent CreateDarkAltarEvent()
        {
            var altarSuccess = new AdventureEvent("You feel your mana surge as the altar consumes your pain.", c =>
            {
                c.CurrentHP = Math.Max(1, c.CurrentHP - 5);
                c.HealMP(c.MaxMana);
            });

            var altarFail = new AdventureEvent("You step away, uncertain of the altar's power.");

            return EventBuilder
                .Describe("You kneel before a dark altar. Its aura tempts you...")
                .WithChanceOutcome(0.5, new[] { altarSuccess }, new[] { altarFail })
                .Build();
        }
        
        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("You discover branching paths in the ancient crypt...")
                .AddExit("A mysterious staircase rises, impossibly, toward the sky.", "FloatingRuins", 0.1)
                .AddExit("You emerge into a quiet village square.", "Village", 0.1)
                .AddExit("A cracked stairway ascends toward a regal chamber.", "RoyalMansion", 0.2)
                .MainExit("You descend deeper and find yourself in the Dark Cave.", "DarkCave")
                .Build(rand);
        }


        private AdventureEvent CreateRareEvent(string description, Action<Character>? onTrigger, double chance)
        {
            return new AdventureEvent(description, onTrigger)
            {
                Eligibility = _ => rand.NextDouble() < chance
            };
        }
    }
}
