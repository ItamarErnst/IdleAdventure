namespace IdleAdventure;
public class AdventureEvent
{
    public string Description { get; set; }
    public string DescriptionColor { get; }
    public Action<Character>? Outcome { get; set; }
    public string? AreaTransition { get; set; }

    public List<NextEventLink> NextEvents { get; } = new();
    public Func<Character, bool>? Eligibility { get; set; }

    public AdventureEvent(
        string description,
        Action<Character>? outcome = null,
        string? areaTransition = null,
        string descriptionColor = Colors.Description)
    {
        Description = description;
        Outcome = outcome;
        AreaTransition = areaTransition;
        DescriptionColor = descriptionColor;
    }

    public virtual void Execute(Character character)
    {
        if (!string.IsNullOrEmpty(Description))
        {
            ColorText.WriteLine(Description, DescriptionColor);
        }

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
