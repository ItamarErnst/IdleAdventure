namespace IdleAdventure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static async Task Main(string[] args)
    {
        bool isPaused = false;

        while (true)
        {
            Character character = StartGame();

            SaveData.SaveGame(character);
            var screenManager = new ScreenManager();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Welcome to the Idle Adventure! ===");
            Console.ResetColor();
            screenManager.ShowCharacterInfo(character);

            var adventureManager = new AdventureManager(character);
            var adventureTask = adventureManager.RunAsync();

            bool isDead = false;
            AdventureManager.OnPlayerDeath += () => isDead = true;

            _ = Task.Run(() =>
            {
                while (!isDead)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.P)
                        {
                            isPaused = !isPaused;
                            adventureManager.TogglePause();
                            if (isPaused)
                            {
                                screenManager.ShowCharacterInfo(character);
                            }
                        }
                    }

                    Thread.Sleep(100);
                }
            });

            await adventureTask;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n☠ You have died! Press any key to start a new adventure...");
            Console.ResetColor();
            Console.ReadKey(true);
            Console.Clear();
        }
    }

    private static Character StartGame()
    {
        // Console.Clear();
        Console.WriteLine("Welcome to Idle Adventure!");
        Console.WriteLine("[L]oad game or [N]ew game?");
        ConsoleKey choice = Console.ReadKey(true).Key;

        Character character;

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
                character = CreateNewCharacter();
            }
        }
        else
        {
            character = CreateNewCharacter();
        }

        return character;
    }

    private static Character CreateNewCharacter()
    {
        var character = CharacterGenerator.Generate();
        var weapon = WeaponFactory.CreateRandom();
        character.Inventory.SetStartingInventory(weapon, 10);
        return character;
    }

}
