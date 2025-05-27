namespace IdleAdventure.Areas
{
    public static class AreaFactory_Cave
    {
        public static Area Create()
        {
            var cave = new Area("Dark Cave")
            {
                EntranceMessage = "You step cautiously into the dark, damp cave..."
            };

            var rand = Random.Shared;

            // 🔹 PATH
            var cavePath = new PathEvent(null, new[]
            {
                "You hear water dripping in the dark.",
                "A small mouse darts across the floor.",
                "You duck to avoid a low-hanging stalactite.",
                "You feel something crunch beneath your boots.",
                "A cold breeze brushes your neck.",
                "You follow a twisting cave path...",
                "Your torch flickers ominously."
            });

            // 🔹 HEALING
            var glowingCrystal = new AdventureEvent(
                "A mysterious glowing crystal pulses with warm light.",
                c => c.HealMP(6))
            {
                Eligibility = c => c.MaxMana < c.CurrentMP
            };

            // 🔹 TREASURE — chance based
            var treasureChest = new AdventureEvent(
                "You find a treasure chest filled with old gold coins!",
                c => c.Inventory.AddGold(rand.Next(5, 15)))
            {
                Eligibility = _ => rand.NextDouble() < 0.25 // 25% chance
            };

            // 🔹 RARE EVENTS — very low chance
            var ancientPaintings = new AdventureEvent("You discover ancient cave paintings...")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            var skeletonRemains = new AdventureEvent("You stumble upon the skeleton of a past adventurer.")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            // 🔹 COMBAT
            var combatEvent = EventBuilder
                .Describe("You hear unsettling sounds from the shadows...")
                .WithAction(character =>
                {
                    var bigTreasure = new AdventureEvent("You uncover a rare artifact!", c => c.Inventory.AddGold(50))
                    {
                        Eligibility = _ => Random.Shared.NextDouble() < 0.05
                    };

                    var onWin = new AdventureEvent("Victory rewards your bravery.", c =>
                    {
                        c.GainXP(5);
                        c.Inventory.AddGold(3);
                    });

                    onWin.AddNext(bigTreasure);

                    CombatSystem.Run(
                        character,
                        enemyFactory: EnemyFactory.CreateRandom,
                        onWin: onWin,
                        onLose: new AdventureEvent("You collapse in the darkness...")
                    );
                })
                .Build();

            // 🔹 EXIT
            var exitChance = new ExitEventBuilder("You see a faint light — the cave exit!")
                .AddExit("You exit to the meadow.", "MeadowField", 0.5)
                .AddExit("You step into a cold, ancient chamber...", "ForgottenCrypt", 0.5)
                .AddExit("You enter a nearby village.", "Village", 1.0)
                .Build(rand);

            cavePath.AddNext(exitChance, _ => rand.NextDouble() < 0.15);


            
            cave.AddEvents(
                new WeightedEvent(cavePath, 4),
                new WeightedEvent(combatEvent, 6),
                new WeightedEvent(treasureChest, 2),
                new WeightedEvent(glowingCrystal, 2),
                new WeightedEvent(ancientPaintings, 1),
                new WeightedEvent(skeletonRemains, 1)
            );
            
            return cave;
        }
    }
}
