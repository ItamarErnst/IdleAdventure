namespace IdleAdventure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static async Task Main(string[] args)
    {
        Character character;

        Console.WriteLine("Welcome to Idle Adventure!");
        Console.WriteLine("[L]oad game or [N]ew game?");
        ConsoleKey choice = Console.ReadKey(true).Key;

        if (choice == ConsoleKey.L && File.Exists(SaveData.SaveFilePath))
        {
            var loaded = SaveData.LoadGame();
            if (loaded != null)
            {
                character = loaded;
                Console.WriteLine("Game loaded successfully!");
            }
            else
            {
                Console.WriteLine("Failed to load game. Starting new...");
                character = CharacterGenerator.Generate();
                Weapon weapon = WeaponFactory.CreateRandom();
                character.Inventory.SetStartingInventory(weapon, 10);
            }
        }
        else
        {
            character = CharacterGenerator.Generate();
            Weapon weapon = WeaponFactory.CreateRandom();
            character.Inventory.SetStartingInventory(weapon, 10);
        }

        SaveData.SaveGame(character);
        var screenManager = new ScreenManager();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=== Welcome to the Idle Adventure! ===");
        Console.ResetColor();
        screenManager.ShowCharacterInfo(character);

        // ✅ In Main()
        var adventureManager = new AdventureManager(character);
        var adventureTask = adventureManager.RunAsync();

        _ = Task.Run(() =>
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.P)
                    {
                        adventureManager.TogglePause();
                    }
                }

                Thread.Sleep(100);
            }
        });

        await adventureTask;
    }
}
