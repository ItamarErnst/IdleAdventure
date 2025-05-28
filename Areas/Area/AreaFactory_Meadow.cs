namespace IdleAdventure.Areas
{
    public static class AreaFactory_Meadow
    {
        public static Area Create()
        {
            var meadow = new Area("Meadow Field")
            {
                EntranceMessage = "You step into a bright meadow filled with wildflowers and buzzing insects."
            };

            var rand = Random.Shared;

            // 🔹 PATH
            var path = new PathEvent(null, new[]
            {
                "You hear birds chirping cheerfully.",
                "A butterfly flits past your face.",
                "The tall grass sways gently in the wind.",
                "You pass a patch of colorful wildflowers.",
                "You hear bees buzzing in the distance.",
                "The sun warms your back as you walk.",
                "You follow a dusty road through the meadow."
            });

            // 🔹 HEALING
            var rainbowHeal = new AdventureEvent(
                "A beautiful rainbow arcs across the sky. You feel renewed.",
                c => c.HealHP(6))
            {
                Eligibility = c => c.CurrentHP < c.MaxHP
            };

            // 🔹 TREASURE — chance-based
            var luckyFind = new AdventureEvent(
                "You spot something shiny in the grass — a few dropped coins!",
                c => c.Inventory.AddGold(rand.Next(2, 6)))
            {
                Eligibility = _ => rand.NextDouble() < 0.25
            };

            // 🔹 RARE EVENTS
            var oldCabin = new AdventureEvent("You find an old abandoned hunter's cabin.")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            var deerEncounter = new AdventureEvent("A gentle deer watches you silently before disappearing into the woods.")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            // 🔹 COMBAT
            var combatEvent = new CombatBuilder()
                .OnWin(c =>
                {
                    c.GainXP(rand.Next(1, 5));
                    c.Inventory.AddGold(rand.Next(1, 5));
                })
                .WithRareDrop(new AdventureEvent("Among the tall grass, you uncover a delicate flower glowing faintly.", c =>
                    c.Inventory.AddItem("Luminous Petal"))
                {
                    Eligibility = _ => rand.NextDouble() < 0.05
                })
                .Build();

            // 🔹 EXITS
            var exit = new ExitEventBuilder("You notice paths branching away from the meadow.")
                .MainExit("You see a small village in the distance.", "Village")
                .AddExit("You spot a dark cave entrance nearby.", "DarkCave", 0.5)
                .AddExit("You hear a whisper from the woods beyond...", "ForestShrine", 0.3)
                .AddExit("A chill runs down your spine — an overgrown crypt lies hidden in the grass.", "ForgottenCrypt", 0.05)
                .Build(rand);

            meadow.AddEvents(
                new WeightedEvent(path, 5),
                new WeightedEvent(exit, 2)
                // new WeightedEvent(combatEvent, 3),
                // new WeightedEvent(rainbowHeal, 1),
                // new WeightedEvent(luckyFind, 1),
                // new WeightedEvent(oldCabin, 1),
                // new WeightedEvent(deerEncounter, 1)
            );
            
            return meadow;
        }
    }
}
