namespace IdleAdventure;

public static class Colors
{
    public const string Bold = "\u001b[1m";
    public const string Underline = "\u001b[4m";
    public const string Blink = "\u001b[5m";
    public const string Inverse = "\u001b[7m";
    public const string Hidden = "\u001b[8m";
    public const string Strikethrough = "\u001b[9m";
    public const string Faint = "\u001b[2m";
    public const string Italic = "\u001b[3m";
    
    public const string Reset = "\u001b[0m";

    // Descriptive or narrative text
    public const string Description = "\u001b[37m";       // White

    // Player name or dialogue
    public const string Player = "\u001b[93m";            // Bright Yellow (Gold)

    // Health-related text
    public const string HP = "\u001b[91m";                // Bright Red (used instead of dark red)

    // Damage dealt or received
    public const string Damage = "\u001b[31m";            // Red

    // Magical effects or abilities
    public const string Magic = "\u001b[36m";             // Cyan

    // Currency values
    public const string Gold = "\u001b[33m";              // Yellow

    // Picked-up or owned items
    public const string Item = "\u001b[32m";              // Green

    // Healing, buffs, or status gains
    public const string Buff = "\u001b[92m";              // Bright Green

    // Ignored actions, suppressed info
    public const string Ignore = "\u001b[90m";            // Gray

    // Entering a new zone/area
    public const string NewArea = "\u001b[36m";        // Cyan
    
    // Lore, hints, environmental info
    public const string Clue = "\u001b[94m";              // Bright Blue

    // System alerts or highlights
    public const string System = "\u001b[97m";            // Bright White
    
    public const string Stats = "\u001b[32m";           // Green
    
    public const string Critical = "\u001b[95m"; // Bright Magenta (feels intense and special)
}
