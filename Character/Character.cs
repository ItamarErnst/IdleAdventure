using System.Drawing;
using System.Text;

namespace IdleAdventure;

public class Character
{
    public StatCalculator Stats { get; private set; }

    public string CurrentArea { get; set; }
    public DateTime? LastDeathTime { get; set; } = DateTime.Now;
    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int XPToNextLevel => Level * 100;

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
    public int MaxMana { get; set; } = 0;

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
        
        Stats = new StatCalculator(this);
        UpdateDerivedStats();
    }
    
    public Weapon GetEquippedWeapon()
    {
        return Inventory.GetWeapon();
    }
    
    private void UpdateDerivedStats()
    {
        MaxHP = Stats.MaximumHP;
        MaxMana = Stats.MaximumMP;
        
        // Initialize HP/MP if they're 0 (new character)
        if (CurrentHP == 0) CurrentHP = MaxHP;
        if (CurrentMP == 0) CurrentMP = MaxMana;
    }
    
    public void GainXP(int amount)
    {
        CurrentXP += amount * 3;
        ColorText.WriteLine($"{Colors.Bold}Gained {amount} XP!", Colors.Player);

        while (CurrentXP >= XPToNextLevel)
        {
            CurrentXP -= XPToNextLevel;
            LevelUp();
        }
        
        ColorText.WriteLine(GetXpBar(CurrentXP, XPToNextLevel), Colors.Player);
    }

    private void LevelUp()
    {
        Level++;
        ColorText.WriteLine($"Level Up! You are now level {Level}.", Colors.Player);

        int statPoints = 3;
        DistributeStatPoints(statPoints);
        UpdateDerivedStats();

        HealFull();
    }

    private void DistributeStatPoints(int points)
    {
        var stats = new List<Action>
        {
            () => Strength++,
            () => Agility++,
            () => Endurance++,
            () => Intelligence++,
            () => Charisma++,
            () => Perception++,
            () => Luck++,
            () => Evasion++
        };

        for (int i = 0; i < points; i++)
        {
            int index = Random.Shared.Next(stats.Count);
            stats[index]();
        }

        ColorText.WriteLine($"+{points} stat points distributed!", Colors.Stats);
    }
    
    public string GetXpBar(int current, int max, int barWidth = 40)
    {
        double ratio = (double)current / max;
        int filled = (int)(ratio * barWidth);
        string percent = $"{(int)(ratio * 100)}%";

        int totalBarLength = barWidth;
        int percentStart = (barWidth - percent.Length) / 2;

        var bar = new StringBuilder();
        for (int i = 0; i < totalBarLength; i++)
        {
            if (i == percentStart)
            {
                bar.Append(percent);
                i += percent.Length - 1;
            }
            else
            {
                bar.Append(i < filled ? '█' : '░');
            }
        }

        return $"[{bar}]";
    }

    
    public void HealFull()
    {
        int hpHealed = MaxHP - CurrentHP;
        int manaHealed = CurrentMP - MaxMana;

        CurrentHP = MaxHP;
        MaxMana = CurrentMP;

        if (hpHealed != 0 || manaHealed != 0)
        {
            ColorText.WriteLine($"Fully healed: +{hpHealed} HP, +{manaHealed} Mana", Colors.Player);
            ColorText.WriteLine($"Current HP: {CurrentHP}/{MaxHP} | Mana: {MaxMana}/{CurrentMP}", Colors.Stats);
        }
    }

    public void HealHP(int amount)
    {
        int before = CurrentHP;
        CurrentHP = Math.Min(CurrentHP + amount, MaxHP);
        int healed = CurrentHP - before;

        if (healed != 0)
        {
            ColorText.WriteLine($"Healed HP: +{healed}", Colors.HP);
            ColorText.WriteLine($"Current HP: {CurrentHP}/{MaxHP}", Colors.Stats);
        }
    }

    public void HealMP(int amount)
    {
        int before = MaxMana;
        MaxMana = Math.Min(MaxMana + amount, CurrentMP);
        int healed = MaxMana - before;

        if (healed != 0)
        {
            ColorText.WriteLine($"Restored Mana: +{healed}", Colors.Magic);
            ColorText.WriteLine($"Current Mana: {MaxMana}/{CurrentMP}", Colors.Stats);
        }
    }
}