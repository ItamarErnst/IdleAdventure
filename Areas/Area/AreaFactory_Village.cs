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

            var fluff = new[]
            {
                "You hear laughter from a nearby tavern.",
                "A cat brushes past your leg and vanishes into an alley.",
                "The smell of fresh bread wafts from a bakery.",
                "Children play in the street nearby.",
                "A street performer is playing a lute softly.",
                "You pass a flower vendor arranging her bouquet.",
                "You walk down a cobblestone street lined with small shops.",
            };

            var street = new PathEvent( null, fluff);

            var npcTalk = EventBuilder
                .Describe("A villager greets you warmly and offers a tale.")
                .Build();

            var gold = EventBuilder
                .Describe("You notice some gold coins lying on the ground!")
                .WithAction(c=> c.Inventory.AddGold(1))
                .Build();
            
            var inn = EventBuilder
                .Describe("You entered a the Inn")
                .WithAction(c=> c.HealFull())
                .Build();

            var rare1 = EventBuilder
                .Describe("A traveling bard performs a lively tune.")
                .Build();

            var rare2 = EventBuilder
                .Describe("You help a child find their lost pet.")
                .Build();

            var exitToMeadow = EventBuilder
                .Describe("You reached the edge of the village and you see the meadow field.")
                .WithTransition("MeadowField", 0.7)
                .Build();
            
            var healingFountain = new AdventureEvent(
                "You find a sparkling healing fountain.",
                c => c.HealHP(10)
            )
            {
                Condition = c => c.CurrentHP < c.MaxHP / 2
            };

            village.AddEvents(new AdventureEvent[]
            {
                street,street,street,
                npcTalk,
                gold,
                inn
            });
            
            return village;
        }
    }
}