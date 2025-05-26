namespace IdleAdventure;

public class Inventory
{
    public List<Weapon> weapons = new();
    public int Gold { get; set; } = 0;
    public Weapon EquippedWeapon { get; set; } = WeaponFactory.CreateUnarmed();
    
    public Weapon GetWeapon()
    {
        return EquippedWeapon;
    }

    public void SetStartingInventory(Weapon weapon, int gold)
    {
        weapons.Add(weapon);
        EquipWeapon(weapon);
        Gold += gold;
    }
    
    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        if (EquippedWeapon.Name == "Unarmed")
        {
            EquipWeapon(weapon);
            ColorText.WriteLine($"You equipped {weapon.Name}.", ConsoleColor.Green);
        }
        else
        {
            ColorText.WriteLine($"You found a new weapon: {weapon.Name}.", ConsoleColor.Cyan);
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        ColorText.WriteLine($"💰 +{amount} gold. Total: {Gold}", ConsoleColor.Yellow);
    }

    public List<string> Show()
    {
        List<string> infoLines = new List<string>();
        infoLines.Add("📦 Inventory:");
        infoLines.Add($"  Gold: {Gold}");
        infoLines.Add($"  Weapons:");
        foreach (var w in weapons)
        {
            string equipped = (w == EquippedWeapon) ? " (Equipped)" : "";
            infoLines.Add($"   • {w.Name}{equipped} [{w.MinDamage}-{w.MaxDamage} dmg]");
        }

        return infoLines;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }
}
