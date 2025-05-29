using System.Text.Json.Serialization;

public class Weapon
{
    public string Name { get; set; } = "Unarmed";
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public double MissChance { get; set; }
    public double CritChance { get; set; }
    public List<string> AttackDescriptions { get; set; } = new();
    
    public Weapon(string name, int minDmg, int maxDmg, double missChance, double critChance, List<string> attackDescs)
    {
        Name = name;
        MinDamage = minDmg;
        MaxDamage = maxDmg;
        MissChance = missChance;
        CritChance = critChance;
        AttackDescriptions = attackDescs;
    }

    public string GetRandomAttackDescription(Random rand)
    {
        return AttackDescriptions[rand.Next(AttackDescriptions.Count)];
    }
}