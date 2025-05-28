using IdleAdventure.Areas;

namespace IdleAdventure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static AreaRegistry registry = new AreaRegistry();

    public static async Task Main(string[] args)
    {
        bool isPaused = false;
        Character? character = null;

        while (true)
        {
            // ✅ If we have a dead character from the previous loop, try to recover
            if (character != null && character.CurrentHP <= 0)
            {
                HandleIdleRecovery(character);

                if (character.CurrentHP > 0)
                {
                    // ✅ Player just recovered — skip character creation
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=== Welcome back to the Idle Adventure! ===");
                    Console.ResetColor();

                    var screenManager = new ScreenManager();
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
                    Console.WriteLine("\n☠ You have died! Preparing to recover...");
                    Console.ResetColor();

                    continue; // next loop will recover or start new
                }
            }

            // ✅ Otherwise start from scratch
            character = StartGame();
            SaveData.SaveGame(character);

            var screen = new ScreenManager();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Welcome to the Idle Adventure! ===");
            Console.ResetColor();
            screen.ShowCharacterInfo(character);

            var manager = new AdventureManager(character);
            var task = manager.RunAsync();

            bool dead = false;
            AdventureManager.OnPlayerDeath += () => dead = true;

            _ = Task.Run(() =>
            {
                while (!dead)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.P)
                        {
                            isPaused = !isPaused;
                            manager.TogglePause();
                            if (isPaused)
                            {
                                screen.ShowCharacterInfo(character);
                            }
                        }
                    }

                    Thread.Sleep(100);
                }
            });

            await task;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n☠ You have died! Preparing to recover...");
            Console.ResetColor();
        }
    }


    private static Character StartGame()
    {
        ShowStartMessage();
        ConsoleKey choice = Console.ReadKey(true).Key;

        Character character = (choice == ConsoleKey.L && File.Exists(SaveData.SaveFilePath))
            ? TryLoadGame()
            : CreateNewCharacter();

        HandleIdleRecovery(character);
        return character;
    }

    private static void ShowStartMessage()
    {
        Console.WriteLine("Welcome to Idle Adventure!");
        Console.WriteLine("[L]oad game or [N]ew game?");
    }

    private static Character TryLoadGame()
    {
        var loaded = SaveData.LoadGame();
        if (loaded != null)
        {
            Console.WriteLine("Game loaded successfully!");
            return loaded;
        }

        Console.WriteLine("Failed to load game. Starting new...");
        return CreateNewCharacter();
    }

    private static void HandleIdleRecovery(Character character)
    {
        if (!character.LastDeathTime.HasValue) return;

        TimeSpan timeSinceDeath = DateTime.UtcNow - character.LastDeathTime.Value;

        if (timeSinceDeath >= TimeSpan.FromMinutes(5))
        {
            FullyHealCharacter(character);
        }
        else if (character.CurrentHP <= 0)
        {
            ShowRecoveryCountdown(character);
        }
    }

    private static void FullyHealCharacter(Character character)
    {
        character.CurrentHP = character.MaxHP;
        character.CurrentArea = registry.GetRandomRecoveryArea();
        character.LastDeathTime = null;
        ColorText.WriteLine("⏳ You have returned fully healed from your rest!");
        SaveData.SaveGame(character);
    }
    
    private static void ShowRecoveryCountdown(Character character)
    {
        int msgLine = Console.CursorTop; // Capture current line to print from

        Console.WriteLine("🪦 You are recovering... Please wait.");
        Console.WriteLine(); // Placeholder for countdown

        while (true)
        {
            if (!character.LastDeathTime.HasValue)
                return;

            TimeSpan timeLeft = character.LastDeathTime.Value.AddMinutes(5) - DateTime.UtcNow;

            if (timeLeft <= TimeSpan.Zero)
            {
                FullyHealCharacter(character);
                Console.SetCursorPosition(0, msgLine + 1);
                Console.WriteLine("💪 You feel your strength return! You are fully healed.       ");
                Thread.Sleep(1500);
                break;
            }

            Console.SetCursorPosition(0, msgLine + 1); // Line after the message
            Console.Write($"⏳ Time remaining: {timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}       ");
            Thread.Sleep(1000);
        }
    }

    
    private static Character CreateNewCharacter()
    {
        var character = CharacterGenerator.Generate();
        var weapon = WeaponFactory.CreateRandom();
        character.CurrentArea = registry.GetRandomStartingArea();
        character.Inventory.SetStartingInventory(weapon, 10);
        
        return character;
    }

}
