namespace IdleAdventure;

public class AdventureEvent
{ 
    public string Description { get; set; }
    public Action<Character>? Outcome { get; set; }
    public List<AdventureEvent> FollowUps { get; set; }
    public string? AreaTransition { get; set; }

    public AdventureEvent(string description, Action<Character>? outcome = null, string? areaTransition = null)
    {
        Description = description;
        Outcome = outcome;
        FollowUps = new List<AdventureEvent>();
        AreaTransition = areaTransition;
    }

    public AdventureEvent GetRandomFollowUp(Random rand)
    {
        if (FollowUps.Count == 0) return this;
        return FollowUps[rand.Next(FollowUps.Count)];
    }

    public virtual void Execute(Character character)
    {
        ColorText.WriteLine(Description, ConsoleColor.White);
        Outcome?.Invoke(character);
    }
}
