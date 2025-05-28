namespace IdleAdventure;

class ScreenManager
{
    private int originalCursorTop;
    private int originalCursorLeft;

    public void ShowPlayerStats(Character character)
    {
        ShowCharacterInfo(character);
        Console.WriteLine("\nPress 'P' again to resume...");
    }

    public void ShowCharacterInfo(Character character)
    {
        originalCursorTop = Console.CursorTop;
        originalCursorLeft = Console.CursorLeft;

        List<string> infoLines = new List<string>
        {
            $"{Colors.Stats}Strength     {character.Strength,2}{Colors.Description}  | Name: {character.Name}",
            $"{Colors.Stats}Agility      {character.Agility,2}{Colors.Description}  | Gender: {character.Gender}",
            $"{Colors.Stats}Endurance    {character.Endurance,2}{Colors.Description}  | Height: {character.Height} cm",
            $"{Colors.Stats}Intelligence {character.Intelligence,2}{Colors.Description}  | Weight: {character.Weight} kg",
            $"{Colors.Stats}Charisma     {character.Charisma,2}{Colors.Description}  | Body: {character.BodyType}",
            $"{Colors.Stats}Perception   {character.Perception,2}{Colors.Description}  | Profession: {character.Profession}",
            $"{Colors.Stats}Luck         {character.Luck,2}{Colors.Description}  | Likes: {string.Join(", ", character.Likes)}",
            $"{Colors.Stats}Evasion      {character.Evasion,2}{Colors.Description}  | Dislikes: {string.Join(", ", character.Dislikes)}",
            $"{Colors.HP}HP           {character.CurrentHP,2}/{character.MaxHP,-3}{Colors.Description}",
            $"{Colors.Magic}Mana           {character.CurrentMP,2}/{character.MaxMana,-3}{Colors.Description}",
            ""
        };

        infoLines.AddRange(character.Inventory.Show());

        int boxWidth = infoLines.Max(line => ConsoleUtils.VisibleLength(line)) + 4;

        string borderTop    = $"╔{new string('═', boxWidth - 2)}╗";
        string borderBottom = $"╚{new string('═', boxWidth - 2)}╝";
        string sideBar      = "║";

        Console.WriteLine(borderTop);

        foreach (string line in infoLines)
        {
            int visibleLength = ConsoleUtils.VisibleLength(line);
            int paddingNeeded = boxWidth - 4 - visibleLength;
            Console.WriteLine($"{sideBar} {line}{new string(' ', paddingNeeded)} {sideBar}");
        }

        Console.WriteLine(borderBottom);
    }

}