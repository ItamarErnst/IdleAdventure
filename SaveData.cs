namespace IdleAdventure;

using System;
using System.IO;
using System.Text.Json;

public class SaveData
{
    public static readonly string SaveFilePath = "savegame.json";
    public Character Character { get; set; } = null!;

    public SaveData()
    {
        
    }
    
    public static void SaveGame(Character character)
    {
        var data = new SaveData
        {
            Character = character,
        };

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        });

        File.WriteAllText(SaveFilePath, json);
    }
    
    public static Character? LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Console.WriteLine("⚠️ Save file not found.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(SaveFilePath);
            var data = JsonSerializer.Deserialize<SaveData>(json, new JsonSerializerOptions
            {
                IncludeFields = true
            });

            if (data?.Character == null)
            {
                Console.WriteLine("⚠️ Save file contains invalid character data.");
                return null;
            }

            return data.Character;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"⚠️ Failed to parse save file: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"⚠️ Failed to read save file: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Unexpected error loading save file: {ex.Message}");
        }

        return null;
    }
}
