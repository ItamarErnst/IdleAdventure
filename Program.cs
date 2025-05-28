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
            bool justRecovered = false;

            // Try to load directly if recovering from death
            if (character != null && character.CurrentHP <= 0)
            {
                character = StartGame(skipPrompt: true);
                HandleIdleRecovery(character);

                if (character.CurrentHP > 0)
                {
                    ColorText.WriteLine("💀 You have recovered from death and are ready to continue!");
                    justRecovered = true;
                }
            }
            else
            {
                character = StartGame(); // with menu
                HandleIdleRecovery(character);

                if (character.CurrentHP <= 0)
                {
                    // Already dead, recovery countdown will run inside HandleIdleRecovery
                    SaveData.SaveGame(character);
                    continue; // go to next loop when done recovering
                }
            }

            SaveData.SaveGame(character);
            var screenManager = new ScreenManager();

            if (!justRecovered)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=== Welcome to the Idle Adventure! ===");
                Console.ResetColor();
                screenManager.ShowCharacterInfo(character);
            }
            
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
            Console.WriteLine("\n☠ You have died! Preparing recovery...");
            Console.ResetColor();

            character.LastDeathTime = DateTime.UtcNow;
            SaveData.SaveGame(character);
        }
    }
    private static Character StartGame(bool skipPrompt = false)
    {
        if (!skipPrompt)
        {
            ShowStartMessage();
            ConsoleKey choice = Console.ReadKey(true).Key;

            if (choice == ConsoleKey.L && File.Exists(SaveData.SaveFilePath))
            {
                var loaded = TryLoadGame();

                if (loaded.CurrentHP <= 0)
                {
                    Console.WriteLine("🕯 Your character is dead but can still recover...");
                }

                return loaded;
            }

            return CreateNewCharacter();
        }

        return TryLoadGame(false);
    }

    
    private static void ShowStartMessage()
    {
        ColorText.WriteLine("Welcome to Idle Adventure!");
        ColorText.WriteLine("[L]oad game or [N]ew game?");
    }

    private static Character TryLoadGame(bool display_load_message = true)
    {
        var loaded = SaveData.LoadGame();
        if (loaded != null)
        {
            if(display_load_message) ColorText.WriteLine("Game loaded successfully!");
            return loaded;
        }

        ColorText.WriteLine("Failed to load game. Starting new...");
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

        ColorText.WriteLine("🪦 You are recovering... Please wait.");

        while (true)
        {
            if (!character.LastDeathTime.HasValue)
                return;

            TimeSpan timeLeft = character.LastDeathTime.Value.AddMinutes(5) - DateTime.UtcNow;

            if (timeLeft <= TimeSpan.Zero)
            {
                FullyHealCharacter(character);
                Console.SetCursorPosition(0, msgLine + 1);
                ColorText.WriteLine("💪 You feel your strength return! You are fully healed.       ");
                Thread.Sleep(1500);
                break;
            }

            Console.SetCursorPosition(0, msgLine + 1); // Line after the message
            ColorText.Write($"⏳ Time remaining: {timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}       ");
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
