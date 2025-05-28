using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public static class AreaFactory_ForgottenCrypt
    {
        public static Area Create()
        {
            var crypt = new Area("ForgottenCrypt")
            {
                EntranceMessage = "You descend into a forgotten crypt, the air stale with death."
            };

            var rand = Random.Shared;

            // 🔹 PATH
            var path = new PathEvent(null, new[]
            {
                "Your footsteps echo ominously down the stone corridor.",
                "Dust and cobwebs coat every surface.",
                "You hear a distant clatter of bone.",
                "You light a rusted wall torch to illuminate the way.",
                "Moss-covered tombs line the narrow path."
            });

            // 🔹 HEALING (chance-based)
            var altarSuccess = new AdventureEvent("You feel your mana surge as the altar consumes your pain.", c =>
            {
                c.CurrentHP = Math.Max(1, c.CurrentHP - 5);
                c.HealMP(c.MaxMana);
            });

            var altarFail = new AdventureEvent("You step away, uncertain of the altar's power.");

            var darkAltar = EventBuilder
                .Describe("You kneel before a dark altar. Its aura tempts you...")
                .WithChanceOutcome(0.5, new[] { altarSuccess }, new[] { altarFail })
                .Build();

            // 🔹 TREASURE
            var boneCharm = new AdventureEvent("Among the bones, you discover a mysterious charm.", c => c.Inventory.AddItem("Bone Charm"))
            {
                Eligibility = _ => rand.NextDouble() < 0.15
            };

            var cursedRelic = new AdventureEvent("You find a cursed relic that pulses with dark energy.", c => c.Inventory.AddItem("Cursed Relic"))
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            };

            // 🔹 COMBAT
            var combatEvent = new CombatBuilder()
                .OnWin(c =>
                {
                    c.GainXP(rand.Next(4, 8));
                    c.Inventory.AddGold(rand.Next(5, 10));
                })
                .WithRareDrop(new AdventureEvent("You find a rare obsidian pendant hidden in the bone pile.", c =>
                    c.Inventory.AddItem("Obsidian Pendant"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();

            // 🔹 EXIT
            var exit = new ExitEventBuilder("You discover branching paths in the ancient crypt...")
                .AddExit("You emerge into a quiet village square.", "Village", 0.1)
                .MainExit("You descend deeper and find yourself in the Dark Cave.", "DarkCave")
                .Build(rand);

            path.AddNext(exit, _ => rand.NextDouble() < 0.1);

            crypt.AddEvents(
                new WeightedEvent(path, 10),
                new WeightedEvent(combatEvent, 6),
                new WeightedEvent(darkAltar, 2),
                new WeightedEvent(boneCharm, 1),
                new WeightedEvent(cursedRelic, 1)
            );

            return crypt;
        }
    }
}
