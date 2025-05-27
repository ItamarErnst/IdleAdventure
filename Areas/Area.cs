namespace IdleAdventure.Areas;

public class Area
{
    public string Name { get; set; }
    public string EntranceMessage { get; set; } = "";
    private readonly List<WeightedEvent> _weightedEvents = new();
    
    public Area(string name)
    {
        Name = name;
        _weightedEvents = new List<WeightedEvent>();
    }

    public void AddEvents(params WeightedEvent[] events)
    {
        _weightedEvents.AddRange(events);
    }
    
    public AdventureEvent GetNextEvent(Character character, AdventureEvent? current, Random rand)
    {
        var eligible = _weightedEvents
            .Where(w => w.Event.IsEligible(character))
            .ToList();

        if (eligible.Count == 0)
            throw new Exception($"No eligible events in area '{Name}'.");

        int totalWeight = eligible.Sum(w => w.Weight);
        int roll = rand.Next(totalWeight);

        int cumulative = 0;
        foreach (var w in eligible)
        {
            cumulative += w.Weight;
            if (roll < cumulative)
                return w.Event;
        }

        // fallback (shouldn't happen)
        return eligible[rand.Next(eligible.Count)].Event;
    }

}
