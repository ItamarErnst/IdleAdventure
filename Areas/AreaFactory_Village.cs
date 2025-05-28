using System;
using System.Collections.Generic;
using IdleAdventure.Areas;

namespace IdleAdventure.AreaFactories
{
    public class AreaFactory_Village : IAreaFactory
    {
        private readonly Random rand = Random.Shared;

        public Area Create()
        {
            var village = new Area("Village", "Village")
            {
                EntranceMessage = $"{Colors.Bold}{Colors.NewArea}You arrive at a lively village with bustling streets and friendly faces.{Colors.Reset}"
            };

            village.AddEvents(
                new WeightedEvent(CreatePathEvent(), 6),
                new WeightedEvent(CreateLocalChat(), 3),
                new WeightedEvent(CreateExitEvent(), 2),
                new WeightedEvent(CreateInnEncounter(), 2),
                new WeightedEvent(CreateStreetCoins(), 2),
                new WeightedEvent(CreateRareEvent("A traveling bard performs a lively tune."), 1),
                new WeightedEvent(CreateRareEvent("You help a child find their lost pet."), 1)
            );

            return village;
        }

        private PathEvent CreatePathEvent()
        {
            var path = new PathEvent(null, new[]
            {
                "You hear laughter from a nearby tavern.",
                "A cat brushes past your leg and vanishes into an alley.",
                "The smell of fresh bread wafts from a bakery.",
                "Children play in the street nearby.",
                "A street performer is playing a lute softly.",
                "You pass a flower vendor arranging her bouquet.",
                "You walk down a cobblestone street lined with small shops."
            });

            path.AddNext(CreateExitEvent(), _ => rand.NextDouble() < 0.1);
            return path;
        }

        private AdventureEvent CreateInnEncounter()
        {
            var innHealed = new AdventureEvent(
                "You rest comfortably in the inn, fully restored.",
                c => c.HealFull());

            var innRefused = new AdventureEvent("The innkeeper shakes his head. 'Maybe next time, traveler.'");

            var innDecision = new AdventureEvent("The innkeeper offers you a warm bed for 5 gold.")
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

            // fallback
            innDecision.AddNext(innRefused);

            return innDecision;
        }

        private AdventureEvent CreateStreetCoins()
        {
            return new AdventureEvent(
                "You notice a few coins lying on the ground.",
                c => c.Inventory.AddGold(1))
            {
                Eligibility = _ => rand.NextDouble() < 0.3
            };
        }

        private AdventureEvent CreateLocalChat()
        {
            return new AdventureEvent("A villager greets you warmly and offers a tale.");
        }

        private AdventureEvent CreateRareEvent(string description)
        {
            return new AdventureEvent(description)
            {
                Eligibility = _ => rand.NextDouble() < 0.1
            };
        }

        private AdventureEvent CreateExitEvent()
        {
            return new ExitEventBuilder("You find a narrow trail heading toward the meadows.")
                .AddExit("A winding road climbs toward snow-covered cliffs.", "SnowyMountain", 0.1)
                .AddExit("You take a wrong turn and descend into the Forgotten Crypt...", "ForgottenCrypt", 0.03)
                .MainExit("You reach the edge of the village and see the meadow field.", "MeadowField")
                .Build(rand);
        }

    }
}
