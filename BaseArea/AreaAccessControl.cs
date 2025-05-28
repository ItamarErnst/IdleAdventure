using IdleAdventure;

public class AreaRequirement
{
    public string StatName { get; set; } = "Strength";
    public int MinimumValue { get; set; } = 0;
    public string FailureMessage { get; set; } = "";

    public bool CheckRequirement(Character character)
    {
        return StatName switch
        {
            "Strength" => character.Strength >= MinimumValue,
            "Agility" => character.Agility >= MinimumValue,
            "Endurance" => character.Endurance >= MinimumValue,
            "Intelligence" => character.Intelligence >= MinimumValue,
            "Charisma" => character.Charisma >= MinimumValue,
            "Perception" => character.Perception >= MinimumValue,
            "Luck" => character.Luck >= MinimumValue,
            "Evasion" => character.Evasion >= MinimumValue,
            _ => true
        };
    }
}