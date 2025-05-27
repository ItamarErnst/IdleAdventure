namespace IdleAdventure;

public class AdventureEvent
{ 
    public string Description { get; set; }
    public Action<Character>? Outcome { get; set; }
    public string? AreaTransition { get; set; }
    public Func<Character, bool>? Condition { get; set; }  // ← NEW

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
    
    public bool IsEligible(Character character) => Condition?.Invoke(character) ?? true;
}
