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
            $"{ColorText.Green}Strength     {character.Strength,2}{ColorText.White}  | Name: {character.Name}",
            $"{ColorText.Green}Agility      {character.Agility,2}{ColorText.White}  | Gender: {character.Gender}",
            $"{ColorText.Green}Endurance    {character.Endurance,2}{ColorText.White}  | Height: {character.Height} cm",
            $"{ColorText.Green}Intelligence {character.Intelligence,2}{ColorText.White}  | Weight: {character.Weight} kg",
            $"{ColorText.Green}Charisma     {character.Charisma,2}{ColorText.White}  | Body: {character.BodyType}",
            $"{ColorText.Green}Perception   {character.Perception,2}{ColorText.White}  | Profession: {character.Profession}",
            $"{ColorText.Green}Luck         {character.Luck,2}{ColorText.White}  | Likes: {string.Join(", ", character.Likes)}",
            $"{ColorText.Green}Evasion      {character.Evasion,2}{ColorText.White}  | Dislikes: {string.Join(", ", character.Dislikes)}",
            $"{ColorText.Blue}HP           {character.CurrentHP,2}/{character.MaxHP,-3}{ColorText.White} | Mana: {character.MaxMana} MP",
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