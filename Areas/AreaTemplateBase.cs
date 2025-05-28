namespace IdleAdventure.Areas;
using System;
using System.Collections.Generic;

public abstract class AreaTemplateBase
{
    protected abstract string AreaName { get; }
    protected abstract string EntranceMessage { get; }
    protected virtual int PathWeight => 5;
    protected virtual int CombatWeight => 3;
    protected virtual int ExitWeight => 2;

    protected virtual Random Rand => Random.Shared;

    public Area Create()
    {
        var area = new Area(AreaName)
        {
            EntranceMessage = EntranceMessage
        };

        area.AddEvents(
            new WeightedEvent(CreatePathEvent(), PathWeight),
            new WeightedEvent(CreateCombatEvent(), CombatWeight),
            new WeightedEvent(CreateExitEvent(), ExitWeight)
        );

        foreach (var special in CreateSpecialEvents())
            area.AddEvents(special);

        return area;
    }

    // 📜 Override for area-specific flavor text
    protected abstract string[] GetPathFluffLines();

    protected virtual PathEvent CreatePathEvent()
    {
        return new PathEvent(null, GetPathFluffLines());
    }

    protected virtual IEnumerable<WeightedEvent> CreateSpecialEvents()
    {
        yield break; // optional
    }

    protected virtual AdventureEvent CreateExitEvent()
    {
        return new ExitEventBuilder("You see multiple paths before you.")
            .MainExit("You see a trail that leads out of the area.", "NextZone")
            .Build(Rand);
    }

    protected virtual AdventureEvent CreateCombatEvent()
    {
        return new CombatBuilder()
            .OnWin(c =>
            {
                c.GainXP(Rand.Next(1, 4));
                c.Inventory.AddGold(Rand.Next(1, 4));
            })
            .Build();
    }

    // Optional helper for rare passive events
    protected AdventureEvent CreateRare(string description, Action<Character>? onTrigger = null, double chance = 0.1)
    {
        return new AdventureEvent(description, onTrigger)
        {
            Eligibility = _ => Rand.NextDouble() < chance
        };
    }
}
