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
            var cryptPath = new PathEvent(null, new[]
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
            var combatEvent = EventBuilder
                .Describe("An undead warrior blocks your path...")
                .WithAction(character =>
                {
                    var onWin = new AdventureEvent("The undead fall. You loot their remains.", c =>
                    {
                        c.GainXP(rand.Next(4, 8));
                        c.Inventory.AddGold(rand.Next(5, 10));
                    });

                    onWin.AddNext(new AdventureEvent("You find a rare obsidian pendant hidden in the bone pile.", c => c.Inventory.AddItem("Obsidian Pendant"))
                    {
                        Eligibility = _ => rand.NextDouble() < 0.05
                    });

                    CombatSystem.Run(
                        character,
                        enemyFactory: EnemyFactory.CreateRandom,
                        onWin: onWin,
                        onLose: new AdventureEvent("You flee into the darkness, barely clinging to life...")
                    );
                })
                .Build();

            // 🔹 EXIT
            // Crypt → Village
            var exitToVillage = new ExitEventBuilder("You find a forgotten tunnel leading toward a distant village.")
                .AddExit("You emerge into a quiet village square.", "Village")
                .Build(rand);
            cryptPath.AddNext(exitToVillage, _ => rand.NextDouble() < 0.05);



            crypt.AddEvents(
                new WeightedEvent(cryptPath, 4),
                new WeightedEvent(combatEvent, 6),
                new WeightedEvent(darkAltar, 2),
                new WeightedEvent(boneCharm, 1),
                new WeightedEvent(cursedRelic, 1)
            );

            return crypt;
        }
    }
}
