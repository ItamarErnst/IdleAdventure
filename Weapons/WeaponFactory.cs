using IdleAdventure;

public static class WeaponFactory
{
    private static readonly Random rand = new();

    private static readonly List<WeaponTemplate> weaponTemplates = new()
    {
        new WeaponTemplate(
            "Rusty Sword", 2, 4, 2, 4, 0.05, 0.15, 0.10, 0.20,
            new() { "slashes clumsily", "swings wildly", "stabs forward" }
        ),
        new WeaponTemplate(
            "Elven Dagger", 1, 3, 1, 3, 0.05, 0.10, 0.20, 0.35,
            new() { "lunges swiftly", "slices precisely", "jabs gracefully" }
        ),
        new WeaponTemplate(
            "Battle Axe", 4, 6, 3, 6, 0.10, 0.20, 0.05, 0.15,
            new() { "cleaves with force", "chops down hard", "spins a brutal arc" }
        ),
        new WeaponTemplate(
            "Fire Wand", 2, 4, 2, 5, 0.10, 0.20, 0.15, 0.30,
            new() { "launches a fireball", "casts a burning spark", "unleashes searing heat" }
        ),
        new WeaponTemplate(
            "Spiked Club", 5, 7, 2, 5, 0.15, 0.25, 0.05, 0.10,
            new() { "smashes down brutally", "bashes with spikes", "slams recklessly" }
        ),
        new WeaponTemplate(
            "Unarmed", 1, 2, 0, 2, 0.15, 0.25, 0.05, 0.10,
            new() { "throws a desperate punch", "kicks awkwardly", "flails wildly" }
        ),
        new WeaponTemplate(
        "Iron Spear", 3, 5, 2, 4, 0.05, 0.10, 0.10, 0.20,
        new() { "thrusts sharply", "pierces through the air", "lunges with reach" }
        ),
        new WeaponTemplate(
            "Poison Dagger", 1, 2, 1, 3, 0.05, 0.10, 0.25, 0.40,
            new() { "nicks with a venomous blade", "strikes swiftly with poison", "scratches and slips away" }
        ),
        new WeaponTemplate(
            "Thunder Hammer", 5, 7, 3, 5, 0.10, 0.20, 0.10, 0.15,
            new() { "slams with electrified fury", "cracks the ground beneath", "unleashes a thunderous blow" }
        ),
        new WeaponTemplate(
            "Crystal Wand", 2, 4, 2, 3, 0.10, 0.20, 0.15, 0.25,
            new() { "channels a shimmering bolt", "casts a beam of focused light", "zaps with crystalline energy" }
        ),
        new WeaponTemplate(
            "Crossbow", 3, 5, 2, 3, 0.05, 0.10, 0.20, 0.30,
            new() { "fires a bolt with a snap", "lets loose a precise shot", "reloads and fires again" }
        ),
        new WeaponTemplate(
            "Twin Blades", 2, 3, 1, 4, 0.05, 0.10, 0.25, 0.35,
            new() { "slashes in a quick combo", "cuts with twin arcs", "whirls around furiously" }
        ),
        new WeaponTemplate(
            "Heavy Flail", 4, 6, 2, 5, 0.10, 0.25, 0.05, 0.10,
            new() { "swings the spiked ball violently", "flings the flail overhead", "crashes down with brute force" }
        ),
        new WeaponTemplate(
            "Frost Staff", 2, 4, 2, 4, 0.10, 0.15, 0.15, 0.25,
            new() { "blasts a freezing wind", "hurls a shard of ice", "chants an icy incantation" }
        ),
        new WeaponTemplate(
            "Hook Blade", 2, 4, 1, 3, 0.05, 0.10, 0.15, 0.30,
            new() { "snags with a curved slice", "hooks and pulls", "slashes with deceptive motion" }
        ),
        new WeaponTemplate(
            "Bone Club", 3, 5, 2, 4, 0.10, 0.20, 0.10, 0.15,
            new() { "clobbers with ancient bone", "swings wildly", "cracks the air with bone" }
        ),
        new WeaponTemplate(
            "Rubber Chicken", 1, 2, 1, 2, 0.10, 0.25, 0.20, 0.35,
            new() { "flops the chicken with a squawk", "slaps you with poultry fury", "wields it like a joke weapon… but it hurts" }
        ),
        new WeaponTemplate(
            "Loaf of Bread", 2, 4, 1, 3, 0.05, 0.15, 0.15, 0.25,
            new() { "delivers a gluten-packed uppercut", "smacks you with stale crust", "crumbs fly as it swings" }
        ),
        new WeaponTemplate(
            "Angry Cat", 1, 3, 1, 4, 0.10, 0.20, 0.20, 0.40,
            new() { "throws a hissing cat at your face", "scratches wildly while yowling", "unleashes feline fury" }
        ),
    };
    
    public static Weapon CreateRandom()
    {
        var template = weaponTemplates[rand.Next(weaponTemplates.Count)];
        return CreateWeapon(template);
    }

    public static Weapon CreateWeapon(WeaponTemplate template)
    {
        int minDmg = rand.Next(template.MinDmgMin, template.MinDmgMax + 1);
        int maxDmg = minDmg + rand.Next(template.MaxDmgBonusMin, template.MaxDmgBonusMax + 1);
        double miss = template.MissMin + rand.NextDouble() * (template.MissMax - template.MissMin);
        double crit = template.CritMin + rand.NextDouble() * (template.CritMax - template.CritMin);

        return new Weapon(template.Name, minDmg, maxDmg, miss, crit, template.Descriptions);
    }
    
    public static Weapon CreateUnarmed()
    {
        var template = new WeaponTemplate(
            "Unarmed", 1, 2, 0, 2, 0.15, 0.25, 0.05, 0.10,
            new() { "throws a desperate punch", "kicks awkwardly", "flails wildly" }
        );
        return CreateWeapon(template);
    }
}