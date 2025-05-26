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

            var fluff = new[]
            {
                "You hear water dripping in the dark.",
                "A small mouse darts across the floor.",
                "You duck to avoid a low-hanging stalactite.",
                "You feel something crunch beneath your boots.",
                "A cold breeze brushes your neck.",
                "You follow a twisting cave path...",
                "Your torch flickers ominously."
            };

            var path = new PathEvent( null, fluff);

            var treasure = EventBuilder
                .Describe("You found a treasure chest filled with gold!")
                .WithAction(c=> c.Inventory.AddGold(Random.Shared.Next(5, 15)))
                .Build();

            var randomEnemy = EnemyFactory.CreateRandom();
            var enemy = EventBuilder
                .Describe(randomEnemy.EncounterText)
                .AsCombat(randomEnemy,
                    onWin: EventBuilder.Describe(randomEnemy.DeathText)
                        .WithAction(c=> c.Inventory.AddGold(Random.Shared.Next(1, 3)))
                        .Build(),
                    onLose: EventBuilder.Describe(randomEnemy.WinText).Build())
                .Build();

            var rare1 = EventBuilder
                .Describe("You discover ancient cave paintings!")
                .Build();

            var rare2 = EventBuilder
                .Describe("A mysterious glowing crystal lights your way.")
                .WithAction(c=> c.HealMP(5))
                .Build();

            var toMeadow = EventBuilder
                .Describe("You exit the cave and arrive at a meadow field.")
                .WithTransition("MeadowField")
                .Build();

            var toVillage = EventBuilder
                .Describe("You exit the cave and enter a nearby village.")
                .WithTransition("Village")
                .Build();
            
            var exit = EventBuilder
                .Describe("You see a faint light — the cave exit!")
                .WithChanceOutcome(0.3, new[] { toMeadow,toVillage }, new[] { path })
                .Build();

            // FollowUps for path
            path.FollowUps.AddRange(new[]
            {
                path,path,
                treasure,
                enemy,
                rare1,
                rare2,
                exit
            });

            cave.Events.AddRange(new AdventureEvent[]
            {
                path,path,path,path,
                enemy,
                rare1,
                rare2,
            });

            return cave;
        }
    }
}
