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
            var path = new PathEvent(null, new[]
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
            var combatEvent = new CombatBuilder()
                .OnWin(c =>
                {
                    c.GainXP(rand.Next(3, 4));
                    c.Inventory.AddGold(rand.Next(1, 6));
                })
                .WithRareDrop(new AdventureEvent("You uncover a treasure chest!", c => c.Inventory.AddGold(50))
                {
                    Eligibility = _ => Random.Shared.NextDouble() < 0.02
                })
                .WithRareDrop(new AdventureEvent("Behind a pile of rubble, you spot a strange gem embedded in the stone.", c =>
                    c.Inventory.AddItem("Cracked Geode"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();
            

            // 🔹 EXIT
            var exit = new ExitEventBuilder("You see a faint light — the cave exit!")
                .MainExit("You exit to the meadow.", "MeadowField")
                .AddExit("You step into a cold, ancient chamber...", "ForgottenCrypt", 0.2)
                .AddExit("You enter a nearby village.", "Village",0.4)
                .Build(rand);
            
            cave.AddEvents(
                new WeightedEvent(path, 10),
                new WeightedEvent(exit, 2),
                new WeightedEvent(combatEvent, 3),
                new WeightedEvent(treasureChest, 1),
                new WeightedEvent(glowingCrystal, 2),
                new WeightedEvent(ancientPaintings, 1),
                new WeightedEvent(skeletonRemains, 1)
            );
            
            return cave;
        }
    }
}
