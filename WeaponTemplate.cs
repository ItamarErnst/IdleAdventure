namespace IdleAdventure;

public class WeaponTemplate
{
    public string Name { get; }
    public int MinDmgMin { get; }
    public int MinDmgMax { get; }
    public int MaxDmgBonusMin { get; }
    public int MaxDmgBonusMax { get; }
    public double MissMin { get; }
    public double MissMax { get; }
    public double CritMin { get; }
    public double CritMax { get; }
    public List<string> Descriptions { get; }

    public WeaponTemplate(string name, int minDmgMin, int minDmgMax, int maxBonusMin, int maxBonusMax,
        double missMin, double missMax, double critMin, double critMax, List<string> descriptions)
    {
        Name = name;
        MinDmgMin = minDmgMin;
        MinDmgMax = minDmgMax;
        MaxDmgBonusMin = maxBonusMin;
        MaxDmgBonusMax = maxBonusMax;
        MissMin = missMin;
        MissMax = missMax;
        CritMin = critMin;
        CritMax = critMax;
        Descriptions = descriptions;
    }
}
