using IdleAdventure;
using IdleAdventure.Areas;
using System;
using System.Collections.Generic;

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

            var fluff = new[]
            {
                "You hear birds chirping cheerfully.",
                "A butterfly flits past your face.",
                "The tall grass sways gently in the wind.",
                "You pass a patch of colorful wildflowers.",
                "You hear bees buzzing in the distance.",
                "The sun warms your back as you walk.",
                "You follow a dusty road through the meadow."
            };

            var road = new PathEvent(null, fluff);

            var seeCave = EventBuilder
                .Describe("You spot a dark cave entrance nearby.")
                .WithTransition("DarkCave", 0.5)
                .Build();

            var seeVillage = EventBuilder
                .Describe("You see a small village in the distance.")
                .WithTransition("Village", 0.5)
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
                .Describe("You find an old abandoned hunter's cabin.")
                .Build();

            var rare2 = EventBuilder
                .Describe("A beautiful rainbow arcs across the sky.")
                .WithAction(c=> c.HealHP(6))
                .Build();

            road.FollowUps.AddRange(new[]
            {
                road,road,
                seeCave,
                seeVillage,
                enemy,
                rare1,
                rare2
            });

            meadow.Events.AddRange(new AdventureEvent[]
            {
                road,road,road,road,
                seeCave,
                seeVillage,
                enemy,
            });

            return meadow;
        }
    }
}
