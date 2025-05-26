namespace IdleAdventure;

public class Character
{
    public string CurrentArea { get; set; }

    public string Name { get; set; }
    public string Gender { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string BodyType { get; set; }
    public string Profession { get; set; }
    public List<string> Likes { get; set; }
    public List<string> Dislikes { get; set; }

    public int CurrentHP { get; set; } = 0;
    public int CurrentMP { get; set; } = 0;
    public int MaxHP { get; set; } = 0;
    public int Mana { get; set; } = 0;

    // RPG Stats
    public int Strength { get; set; } = 5;
    public int Agility { get; set; } = 5;
    public int Endurance { get; set; } = 5;
    public int Intelligence { get; set; } = 5;
    public int Charisma { get; set; } = 5;
    public int Perception { get; set; } = 5;
    public int Luck { get; set; } = 5;
    public int Evasion { get; set; } = 5;

    public Inventory Inventory { get; set; } = new Inventory();

    public Character()
    {
        CurrentArea = "MeadowField";
        Name = "Unknown";
        Gender = "Unknown";
        BodyType = "Average";
        Profession = "None";
        Likes = new List<string>();
        Dislikes = new List<string>();
    }

    public Weapon GetEquippedWeapon()
    {
        return Inventory.GetWeapon();
    }
    
    public void HealFull()
    {
        int hpHealed = MaxHP - CurrentHP;
        int manaHealed = MaxMana() - Mana;

        CurrentHP = MaxHP;
        Mana = MaxMana();

        ColorText.WriteLine($"Fully healed: +{hpHealed} HP, +{manaHealed} Mana", ConsoleColor.Green);
        ColorText.WriteLine($"Current HP: {CurrentHP}/{MaxHP} | Mana: {Mana}/{MaxMana()}", ConsoleColor.DarkGray);
    }

    public void HealHP(int amount)
    {
        int before = CurrentHP;
        CurrentHP = Math.Min(CurrentHP + amount, MaxHP);
        int healed = CurrentHP - before;

        ColorText.WriteLine($"Healed HP: +{healed}", ConsoleColor.Green);
        ColorText.WriteLine($"Current HP: {CurrentHP}/{MaxHP}", ConsoleColor.DarkGray);
    }

    public void HealMP(int amount)
    {
        int before = Mana;
        Mana = Math.Min(Mana + amount, MaxMana());
        int healed = Mana - before;

        ColorText.WriteLine($"Restored Mana: +{healed}", ConsoleColor.Blue);
        ColorText.WriteLine($"Current Mana: {Mana}/{MaxMana()}", ConsoleColor.DarkGray);
    }

    public int MaxMana()
    {
        return CurrentMP; // or use: return 50 + Intelligence * 2;
    }


}