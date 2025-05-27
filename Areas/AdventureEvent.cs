namespace IdleAdventure;
public class AdventureEvent
{
    public string Description { get; set; }
    public Action<Character>? Outcome { get; set; }
    public string? AreaTransition { get; set; }

    public List<NextEventLink> NextEvents { get; } = new();
    public Func<Character, bool>? Eligibility { get; set; }

    public AdventureEvent(string description, Action<Character>? outcome = null, string? areaTransition = null)
    {
        Description = description;
        Outcome = outcome;
        AreaTransition = areaTransition;
    }

    public virtual void Execute(Character character)
    {
        ColorText.WriteLine(Description, ConsoleColor.White);
        Outcome?.Invoke(character);
    }

    public bool IsEligible(Character c) => Eligibility?.Invoke(c) ?? true;

    public void AddNext(AdventureEvent evt, Func<Character, bool>? condition = null)
    {
        NextEvents.Add(new NextEventLink(evt, condition));
    }

    public AdventureEvent? GetValidNext(Character c, Random rand)
    {
        var valid = NextEvents
            .Where(link => link.IsValid(c) && link.Event.IsEligible(c))
            .ToList();

        return valid.Count > 0 ? valid[rand.Next(valid.Count)].Event : null;
    }
}
