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
            var path = new PathEvent(null, new[]
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
            var combatEvent = new CombatBuilder()
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

            // 🔹 EXIT
            var exit = new ExitEventBuilder("You follow a worn path leading back to the meadows.")
                .AddExit("You stumble into the Forgotten Crypt.", "ForgottenCrypt", 0.1)
                .AddExit("You see a small village through the trees.", "Village", 0.1)
                .AddExit("You find a hidden path to a dark cave.", "DarkCave", 0.1)
                .MainExit("You arrive at the edge of the MeadowField.", "MeadowField")
                .Build(rand);
            
            path.AddNext(exit, _ => rand.NextDouble() < 0.15);

            
            forest.AddEvents(
                new WeightedEvent(path, 10),
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
