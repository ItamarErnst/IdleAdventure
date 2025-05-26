namespace IdleAdventure.Areas;

public class PathEvent : AdventureEvent
{
    private readonly string[] fluffLines;
    private static readonly string[] DefaultFluff = new[]
    {
        "You hear water dripping nearby.",
        "A small animal scurries past your feet.",
        "There’s a fork — you pick a direction at random.",
        "You pause to take a breath.",
        "You brush past something hanging from above.",
        "You hear faint echoes in the distance."
    };

    private static readonly Random Rand = new();

    public PathEvent(Action<Character>? outcome = null, string[]? customFluff = null)
        : base("", outcome)
    {
        fluffLines = customFluff ?? DefaultFluff;
    }

    public override void Execute(Character character)
    {
        var fluff = fluffLines[Rand.Next(fluffLines.Length)];
        ColorText.WriteLine($"{fluff}", ConsoleColor.White);
        Outcome?.Invoke(character);
    }
}