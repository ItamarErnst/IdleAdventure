using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public static class AreaFactory_ForestShrine
    {
        public static Area Create()
        {
            var forest = new Area("ForestShrine")
            {
                EntranceMessage = "You arrive at a serene forest shrine, silent and overgrown."
            };

            var rand = Random.Shared;

            // 🔹 PATH
            var shrinePath = new PathEvent(null, new[]
            {
                "You hear birds chirping in the distance.",
                "Sunlight peeks through thick canopies.",
                "A deer dashes past through the trees.",
                "Leaves rustle softly beneath your feet.",
                "You spot ancient carvings on a stone."
            });

            // 🔹 HEALING
            var mysticPond = new AdventureEvent("You find a clear mystic pond. You feel revitalized as you drink.", c => c.HealFull());

            // 🔹 TREASURE
            var enchantedHerb = new AdventureEvent("You find an enchanted herb glowing beneath a tree.", c => c.Inventory.AddItem("Enchanted Herb"));
            var sacredGem = new AdventureEvent("You unearth a sacred gem from an old altar.", c => c.Inventory.AddItem("Sacred Gem"))
            {
                Eligibility = _ => rand.NextDouble() < 0.2
            };

            // 🔹 RARE
            var rareSpirit = new AdventureEvent("An ancient spirit appears and blesses you with a stat increase!", c => c.Strength += 1)
            {
                Eligibility = _ => rand.NextDouble() < 0.05
            };

            // 🔹 COMBAT
            var combatEvent = EventBuilder
                .Describe("A Forest Guardian emerges from the shadows...")
                .WithAction(character =>
                {
                    var onWin = new AdventureEvent("The guardian falls. You gain experience.", c =>
                    {
                        c.GainXP(rand.Next(3, 6));
                        c.Inventory.AddGold(rand.Next(3, 6));
                    });

                    onWin.AddNext(new AdventureEvent("You find a glowing relic among the ruins.", c => c.Inventory.AddItem("Relic"))
                    {
                        Eligibility = _ => rand.NextDouble() < 0.05
                    });

                    CombatSystem.Run(
                        character,
                        enemyFactory: EnemyFactory.CreateRandom,
                        onWin: onWin,
                        onLose: new AdventureEvent("You retreat, wounded and humbled...")
                    );
                })
                .Build();

            // 🔹 EXIT
            

            // Forest Shrine → Meadow
            var exitToMeadow = new ExitEventBuilder("You follow a worn path leading back to the meadows.")
                .AddExit("You arrive at the edge of the MeadowField.", "MeadowField")
                .Build(rand);
            shrinePath.AddNext(exitToMeadow, _ => rand.NextDouble() < 0.1);

            
            
            forest.AddEvents(
                new WeightedEvent(shrinePath, 4),
                new WeightedEvent(combatEvent, 6),
                new WeightedEvent(mysticPond, 2),
                new WeightedEvent(enchantedHerb, 2),
                new WeightedEvent(sacredGem, 1),
                new WeightedEvent(rareSpirit, 1)
            );

            return forest;
        }
    }
}
