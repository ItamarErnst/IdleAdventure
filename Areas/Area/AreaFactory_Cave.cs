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
                        .WithAction(c=> c.GainXP(Random.Shared.Next(1, 5)))
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
                .WithTransition("MeadowField",1)
                .Build();

            var toVillage = EventBuilder
                .Describe("You exit the cave and enter a nearby village.")
                .WithTransition("Village")
                .Build();
            
            var exit = EventBuilder
                .Describe("You see a faint light — the cave exit!")
                .WithAction(c =>
                {
                    if (Random.Shared.NextDouble() < 0.3)
                    {
                        ColorText.WriteLine("You decide to walk toward it...", ConsoleColor.Gray);

                        // 50/50 which area you reach
                        var next = Random.Shared.NextDouble() < 0.5 ? toMeadow : toVillage;
                        next.Execute(c);
                    }
                    else
                    {
                        ColorText.WriteLine("You hesitate and continue exploring instead.", ConsoleColor.DarkGray);
                        Thread.Sleep(GlobalTimer.EventTimer);
                    }
                })
                .Build();

            var path = new PathEvent(null, fluff);
            path.FollowUps.AddRange(new AdventureEvent[]
            {
                path, path, path,
                treasure,
                enemy,
                rare1,
                rare2,
                exit // 👈 inserted here to occasionally trigger
            });


            cave.AddEvents(new AdventureEvent[]
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
