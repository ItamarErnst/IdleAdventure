using System.Text.RegularExpressions;

namespace IdleAdventure
{
    public static class ConsoleUtils
    {
        private static readonly Regex ansiRegex = new(@"\u001b\[[0-9;]*m", RegexOptions.Compiled);

        /// <summary>Returns visible character count (ignores ANSI color codes).</summary>
        public static int VisibleLength(string input)
        {
            return ansiRegex.Replace(input, "").Length;
        }

        /// <summary>Prints centered text based on console width.</summary>
        public static void WriteCentered(string text)
        {
            int width = Console.WindowWidth;
            int padding = (width - VisibleLength(text)) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, padding)) + text);
        }

        /// <summary>Draws a horizontal line across the terminal.</summary>
        public static void DrawSeparator(char character = '─')
        {
            Console.WriteLine(new string(character, Console.WindowWidth));
        }

        /// <summary>Draws a colored horizontal line.</summary>
        public static void DrawFancySeparator(string color = ColorText.Blue)
        {
            string bar = $"{color}{new string('═', Console.WindowWidth)}{ColorText.Reset}";
            Console.WriteLine(bar);
        }

        /// <summary>Prompts the user for input with a colored label.</summary>
        public static string PromptInput(string label, string color = ColorText.Yellow)
        {
            Console.Write($"{color}{label}{ColorText.Reset}: ");
            if (Console.ReadLine() == null)
                return "";
            
            return Console.ReadLine();
        }

        /// <summary>Draws a box around a list of strings (with optional border color).</summary>
        public static void DrawBox(List<string> lines, string borderColor)
        {
            int boxWidth = lines.Max(VisibleLength) + 4;
            string top = "╔" + new string('═', boxWidth - 2) + "╗";
            string bottom = "╚" + new string('═', boxWidth - 2) + "╝";
            string side = "║";

            if (borderColor != null)
            {
                top = $"{borderColor}{top}{ColorText.Reset}";
                bottom = $"{borderColor}{bottom}{ColorText.Reset}";
            }

            Console.WriteLine(top);
            foreach (var line in lines)
            {
                int pad = boxWidth - 4 - VisibleLength(line);
                Console.WriteLine($"{side} {line}{new string(' ', pad)} {side}");
            }
            Console.WriteLine(bottom);
        }

        /// <summary>Writes text one character at a time (typewriter effect).</summary>
        public static void WriteWithDelay(string text, int delayMs = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
            Console.WriteLine();
        }

        /// <summary>Pauses for any keypress with a custom message.</summary>
        public static void WaitForKey(string message = "Press any key to continue...")
        {
            Console.WriteLine($"\n{message}");
            Console.ReadKey(true);
        }
    }
}
