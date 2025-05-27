namespace IdleAdventure.Areas
{
    public static class AreaFactory_Village
    {
        public static Area Create()
        {
            var village = new Area("Village")
            {
                EntranceMessage = "You arrive at a lively village with bustling streets and friendly faces."
            };

            var rand = Random.Shared;

            // 🔹 PATH
            var streetWalk = new PathEvent(null, new[]
            {
                "You hear laughter from a nearby tavern.",
                "A cat brushes past your leg and vanishes into an alley.",
                "The smell of fresh bread wafts from a bakery.",
                "Children play in the street nearby.",
                "A street performer is playing a lute softly.",
                "You pass a flower vendor arranging her bouquet.",
                "You walk down a cobblestone street lined with small shops."
            });

            // 🔹 INN ENCOUNTER (conditional healing chain)
            var innHealed = new AdventureEvent(
                "You rest comfortably in the inn, fully restored.",
                c => c.HealFull()
            );

            var innRefused = new AdventureEvent(
                "The innkeeper shakes his head. 'Maybe next time, traveler.'"
            );

            var innDecision = new AdventureEvent(
                "The innkeeper offers you a warm bed for 5 gold."
            )
            {
                Eligibility = c => c.Inventory.Gold >= 5
            };

            // 75% chance to heal and pay
            innDecision.AddNext(innHealed, c =>
            {
                if (rand.NextDouble() < 0.75)
                {
                    c.Inventory.AddGold(-5);
                    return true;
                }
                return false;
            });

            // 25% or ineligible
            innDecision.AddNext(innRefused);

            // 🔹 COINS ON GROUND (chance-based)
            var streetCoins = new AdventureEvent(
                "You notice a few coins lying on the ground.",
                c => c.Inventory.AddGold(1)
            )
            {
                Eligibility = _ => rand.NextDouble() < 0.3
            };

            // 🔹 FLAVOR
            var localChat = new AdventureEvent("A villager greets you warmly and offers a tale.");

            // 🔹 RARE EVENTS
            var bardSong = new AdventureEvent("A traveling bard performs a lively tune.")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            var lostPet = new AdventureEvent("You help a child find their lost pet.")
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };

            // 🔹 EXIT
            // Village → Meadow
            var exitToMeadow = new ExitEventBuilder("You find a narrow trail heading toward the meadows.")
                .AddExit("You reach the edge of the village and see the meadow field.", "MeadowField", 1.0)
                .Build(rand);
            streetWalk.AddNext(exitToMeadow, c => rand.NextDouble() < 0.1);
            
            village.AddEvents(
                new WeightedEvent(streetWalk, 4),
                new WeightedEvent(innDecision, 3),
                new WeightedEvent(localChat, 3),
                new WeightedEvent(streetCoins, 2),
                new WeightedEvent(bardSong, 1),
                new WeightedEvent(lostPet, 1)
            );

            return village;
        }
    }
}
