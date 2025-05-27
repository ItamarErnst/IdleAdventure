namespace IdleAdventure;

public class NextEventLink
{
    public AdventureEvent Event { get; }
    public Func<Character, bool>? Condition { get; }

    public NextEventLink(AdventureEvent evt, Func<Character, bool>? condition = null)
    {
        Event = evt;
        Condition = condition;
    }

    public bool IsValid(Character c) => Condition?.Invoke(c) ?? true;
}
