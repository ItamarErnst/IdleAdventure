namespace IdleAdventure.Areas;

public class WeightedEvent
{
    public AdventureEvent Event { get; }
    public int Weight { get; }

    public WeightedEvent(AdventureEvent evt, int weight = 1)
    {
        Event = evt;
        Weight = weight;
    }
}
