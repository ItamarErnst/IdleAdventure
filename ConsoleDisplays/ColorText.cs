using IdleAdventure;

public static class ColorText
{
    public static void Write(string text, string foreground = "")
    {
        string format = "";

        if (!string.IsNullOrEmpty(foreground))
            format += foreground;

        Console.Write($"{format}{text}{Colors.Reset}");
    }


    public static void WriteLine(string text, string foreground = "")
    {
        Write(text + "\n", foreground);
    }
}