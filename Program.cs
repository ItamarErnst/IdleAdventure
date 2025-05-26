namespace IdleAdventure;

using System;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        
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
                character.Inventory.SetStartingInventory(weapon,10);
            }
        }
        else
        {
            character = CharacterGenerator.Generate();
            Weapon weapon = WeaponFactory.CreateRandom();
            character.Inventory.SetStartingInventory(weapon,10);
        }

        SaveData.SaveGame(character);
        bool paused = false;
        var screenManager = new ScreenManager();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=== Welcome to the Idle Adventure! ===");
        Console.ResetColor();
        screenManager.ShowCharacterInfo(character);
        
        AdventureManager adventureManager = new AdventureManager(character);
        var adventureTask = adventureManager.RunAsync();
        
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.P)
                {
                    paused = !paused;
                    if (paused)
                    {
                        adventureManager.Pause();
                        screenManager.ShowPauseScreen(character);
                    }
                    else
                    {
                        adventureManager.Resume();
                    }
                }
            }

            Thread.Sleep(100);
        }
    }
}