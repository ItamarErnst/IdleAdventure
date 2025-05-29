using IdleAdventure;

public class Enemy
{
    public string Name { get; }
    public int HP { get; set; }
    public int Mana { get; set; }
    public int Evasion { get; }
    public int MinDamage { get; }
    public int MaxDamage { get; }
    public List<string> AttackDescriptions { get; }
    
    // New stat properties
    public int DodgeChance { get; }
    public int PhysicalDefense { get; }
    public bool IsCriticalHit() => Random.Shared.Next(100) < CriticalHitChance;
    public int CriticalHitChance { get; }
    public int XPValue { get; } = 3;

    public string EncounterText { get; }
    public string DeathText { get; }
    public string WinText { get; }

    public Enemy(
        string name,
        int hp,
        int mana,
        int evasion,
        int minDmg,
        int maxDmg,
        List<string> attackDescs,
        string encounterText,
        string deathText,
        string winText,
        // New constructor parameters
        int dodgeChance = 5,
        int physicalDefense = 10,
        int criticalHitChance = 5) 
    {
        Name = name;
        HP = hp;
        Mana = mana;
        Evasion = evasion;
        MinDamage = minDmg;
        MaxDamage = maxDmg;
        AttackDescriptions = attackDescs;

        EncounterText = encounterText;
        DeathText = deathText;
        WinText = winText;

        // Initialize new stats
        DodgeChance = dodgeChance;
        PhysicalDefense = physicalDefense;
        CriticalHitChance = criticalHitChance;
    }

    public int GetDamage(Random rand)
    {
        int damage = rand.Next(MinDamage, MaxDamage + 1);
        
        // Apply critical hit if rolled
        if (IsCriticalHit())
        {
            damage = (int)(damage * 1.5f);
            ColorText.WriteLine($"{Name} lands a critical hit!", Colors.Critical);
        }
        
        return damage;
    }

    public string GetRandomAttackDescription(Random rand)
    {
        return AttackDescriptions[rand.Next(AttackDescriptions.Count)];
    }
}