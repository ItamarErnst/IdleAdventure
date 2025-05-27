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
            var meadowPath = new PathEvent(null, new[]
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
            var combatEvent = EventBuilder
                .Describe("You hear unsettling sounds from the shadows...")
                .WithAction(character =>
                {
                    var onWin = new AdventureEvent("Victory rewards your bravery.", c =>
                    {
                        c.GainXP(5);
                        c.Inventory.AddGold(3);
                    });
                    
                    CombatSystem.Run(
                        character,
                        enemyFactory: EnemyFactory.CreateRandom,
                        onWin: onWin,
                        onLose: new AdventureEvent("You collapse in the darkness...")
                    );
                })
                .Build();

            // 🔹 EXITS
            // Meadow → Cave + Village
            var meadowExits = new ExitEventBuilder("You notice paths branching away from the meadow.")
                .AddExit("You spot a dark cave entrance nearby.", "DarkCave", 0.1)
                .AddExit("You see a small village in the distance.", "Village", 0.1)
                .Build(rand);
            meadowPath.AddNext(meadowExits);
            
            meadow.AddEvents(
                new WeightedEvent(meadowPath, 5),
                new WeightedEvent(combatEvent, 3),
                new WeightedEvent(rainbowHeal, 1),
                new WeightedEvent(luckyFind, 1),
                new WeightedEvent(oldCabin, 1),
                new WeightedEvent(deerEncounter, 1)
            );
            
            return meadow;
        }
    }
}
