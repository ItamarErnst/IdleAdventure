namespace IdleAdventure;

public class Inventory
{
    public Weapon EquippedWeapon { get; set; } = WeaponFactory.CreateUnarmed();
    public List<Weapon> weapons = new();
    public int Gold { get; set; } = 0;
    
    public Dictionary<string, int> items = new();
    
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

    public void AddItem(string itemName)
    {
        if (items.TryGetValue(itemName, out int count))
        {
            count++;
        }
        else
        {
            items.Add(itemName, 1);
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
        
        infoLines.Add($"  Items:");
        foreach (var i in items)
        {
            infoLines.Add($"   • {i.Key} [{i.Value}]");
        }

        return infoLines;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }
}
