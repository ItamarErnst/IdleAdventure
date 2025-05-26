namespace IdleAdventure.Areas;

public class Area
{
    public string Name { get; set; }
    public string EntranceMessage { get; set; } = "";
    public List<AdventureEvent> Events { get; set; }

    public Area(string name)
    {
        Name = name;
        Events = new List<AdventureEvent>();
    }

    public AdventureEvent GetRandomEvent(Random rand)
    {
        return Events[rand.Next(Events.Count)];
    }
}