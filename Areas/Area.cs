namespace IdleAdventure.Areas;

public class Area
{
    public string Name { get; set; }
    public string EntranceMessage { get; set; } = "";
    private List<AdventureEvent> _events;
    private int _eventIndex = 0;

    public Area(string name)
    {
        Name = name;
        _events = new List<AdventureEvent>();
    }

    public void AddEvents(params AdventureEvent[] events)
    {
        _events.AddRange(events);
    }

    public AdventureEvent GetNextEvent(Character character, Random rand)
    {
        var eligible = _events.Where(e => e.IsEligible(character)).ToList();
        if (eligible.Count == 0)
            throw new Exception($"No eligible events in area '{Name}'.");

        return eligible[rand.Next(eligible.Count)];
    }
}
