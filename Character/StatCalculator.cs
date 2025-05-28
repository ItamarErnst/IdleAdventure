namespace IdleAdventure;

public class StatCalculator
{
    private readonly Character _character;
    private readonly Random _random = Random.Shared;

    public StatCalculator(Character character)
    {
        _character = character;
    }
    
    // Combat Stats
    public int PhysicalDamageBonus => (int)(_character.Strength * 0.5f);
    public int DodgeChance => (int)(_character.Agility * 0.5f + _character.Evasion * 0.5f);
    public int CriticalHitChance => (int)(_character.Luck * 0.3f + _character.Perception * 0.2f); 
    public bool IsCriticalHit() => Random.Shared.Next(100) < CriticalHitChance;

    // Derived HP and MP
    public int MaximumHP => 100 + (_character.Endurance * 10);
    public int MaximumMP => 50 + (_character.Intelligence * 8);
    public float MagicPower => 1 + (_character.Intelligence * 0.05f);
    
    // Defense calculations
    public int PhysicalDefense => (int)(_character.Endurance * 0.7f + _character.Strength * 0.3f);
    public int MagicalDefense => (int)(_character.Intelligence * 0.7f + _character.Endurance * 0.3f);
    
    // Exploration and Events
    public int DetectionRadius => _character.Perception * 2;
    public int TrapDetectionChance => (int)(_character.Perception * 0.6f + _character.Luck * 0.4f);
    public int StealthLevel => (int)(_character.Agility * 0.7f + _character.Perception * 0.3f);
    
    // Social and Economic
    public float MerchantPriceModifier => 1 - (_character.Charisma * 0.01f);
    public int ReputationGainModifier => (int)(_character.Charisma * 0.5f);
    
    // Synergy Bonuses
    public int PhysicalSynergyBonus => (int)((_character.Strength + _character.Endurance) * 0.2f);
    public int MagicalSynergyBonus => (int)((_character.Intelligence + _character.Perception) * 0.2f);
    public int CriticalSynergyBonus => (int)((_character.Agility + _character.Luck) * 0.15f);
    
    // Loot and Rewards
    public float LootChanceModifier() => 1 + (_character.Luck * 0.02f);
    


}