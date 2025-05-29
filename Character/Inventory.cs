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
        if (EquippedWeapon.Name == "Unarmed" || Random.Shared.NextDouble() < 0.25 || EquippedWeapon.MaxDamage < weapon.MaxDamage)
        {
            EquipWeapon(weapon);
            ColorText.WriteLine($"You equipped {Colors.Magic}{weapon.Name}.", Colors.Item);
        }
        else
        {
            ColorText.WriteLine($"You found a new weapon: {Colors.Magic}{weapon.Name}.", Colors.Item);
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
        
        ColorText.WriteLine($"You found {Colors.Magic}{itemName}.", Colors.Item);
    }

    public void AddGold(int amount)
    {
        if(amount < 0) return;
        
        Gold += amount;
        ColorText.WriteLine($"💰 +{amount} gold. Total: {Gold}", Colors.Gold);
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

    public bool HasItem(string item)
    {
        if (items.TryGetValue(item, out int count))
        {
            return count > 0;
        }
       
        return false;
    }

    public void RemoveItem(string item)
    {
        if (items.TryGetValue(item, out int count))
        {
            count--;
        }
    }
}
