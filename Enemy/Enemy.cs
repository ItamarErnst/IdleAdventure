public class Enemy
{
    public string Name { get; }
    public int HP { get; set; }
    public int Mana { get; set; }
    public int Evasion { get; }
    public int MinDamage { get; }
    public int MaxDamage { get; }
    public List<string> AttackDescriptions { get; }

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
        string winText)
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
    }

    public int GetDamage(Random rand)
    {
        return rand.Next(MinDamage, MaxDamage + 1);
    }

    public string GetRandomAttackDescription(Random rand)
    {
        return AttackDescriptions[rand.Next(AttackDescriptions.Count)];
    }
}