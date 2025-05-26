using System.Diagnostics;

namespace IdleAdventure;

public static class CharacterGenerator
{
    static Random rand = new Random();

    static string[] names = {
        "Aeris", "Bran", "Luna", "Kael", "Mira", "Thorne", "Elira", "Dain", "Nyx", "Silas"
    };

    static string[] genders = {
        "Male", "Female", "Non-binary", "Fluid", "Other"
    };

    static string[] bodyTypes = {
        "Slim", "Average", "Athletic", "Heavy", "Lithe", "Bulky"
    };

    static string[] professions = {
        "Wizard", "Warrior", "Thief", "Hunter", "Alchemist", "Bard", "Paladin", "Necromancer", "Merchant", "Monk"
    };

    static string[] likes = {
        "gold", "cats", "magic", "books", "adventure", "cake", "fame", "weapons", "flowers", "strategy"
    };

    static string[] dislikes = {
        "cold", "darkness", "noise", "rules", "bureaucracy", "snakes", "fish", "dust", "waiting", "heat"
    };

    public static Character Generate()
    {
        var c = new Character
        {
            Name = names[rand.Next(names.Length)],
            Gender = genders[rand.Next(genders.Length)],
            Height = rand.Next(150, 200),
            Weight = rand.Next(50, 120),
            BodyType = bodyTypes[rand.Next(bodyTypes.Length)],
            Profession = professions[rand.Next(professions.Length)],
            Likes = likes.OrderBy(_ => rand.Next()).Take(2).ToList(),
            Dislikes = dislikes.OrderBy(_ => rand.Next()).Take(2).ToList(),
            MaxHP = rand.Next(15, 30),
            Mana = rand.Next(0, 25),

            Strength = rand.Next(3, 11),
            Agility = rand.Next(3, 11),
            Endurance = rand.Next(3, 11),
            Intelligence = rand.Next(3, 11),
            Charisma = rand.Next(3, 11),
            Perception = rand.Next(3, 11),
            Luck = rand.Next(3, 11),
            Evasion = rand.Next(3, 11)
        };
        
        c.CurrentHP = c.MaxHP;
        c.CurrentMP = c.Mana;
        return c;
    }
}