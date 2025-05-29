public static class EnemyFactory
{
    private static readonly Random rand = new();

    private static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> templates = new()
    {
        (
            "Goblin",
            new() { "swings a jagged knife!", "bites viciously!" },
            "A sneaky goblin jumps out from the shadows!",
            "The goblin lets out a shriek and collapses.",
            "The goblin cackles triumphantly over your corpse."
        ),
        (
            "Skeleton",
            new() { "slashes with a rusted blade!", "rattles menacingly!" },
            "A skeleton rises from the ground, bones clattering.",
            "The skeleton crumbles into a pile of bones.",
            "The skeleton raises its sword in a silent victory."
        ),
        (
            "Boar",
            new() { "gores with its tusks!", "charges headfirst!" },
            "A wild boar bursts from the underbrush, snorting angrily!",
            "The boar squeals and crashes to the ground.",
            "The boar grunts, victorious and bloodied."
        ),
        (
            "Bat Swarm",
            new() { "swirls around you biting!", "dive-bombs you!" },
            "A cloud of bats erupts from the cave ceiling!",
            "The swarm scatters into the dark.",
            "The swarm engulfs you completely before fading into the night."
        ),
        (
            "Cultist",
            new() { "casts a shadow bolt!", "screeches in tongues!" },
            "A robed cultist appears, chanting madly!",
            "The cultist collapses mid-incantation.",
            "The cultist laughs as your soul fades into the void."
        ),
        (
            "Wraith",
            new() { "reaches with spectral claws!", "lets out a chilling wail!" },
            "A ghostly wraith glides silently toward you...",
            "The wraith shrieks and dissipates into mist.",
            "The wraith hovers over your still body, silent once more."
        ),
        (
            "Slime",
            new() { "splashes acidic goo!", "bounces into you!" },
            "A gelatinous slime oozes from the cracks!",
            "The slime quivers, then melts into the ground.",
            "The slime jiggles victoriously, absorbing your gear."
        ),
        (
            "Bandit",
            new() { "slashes with a dagger!", "throws a poison vial!" },
            "A masked bandit steps from the shadows!",
            "The bandit gasps and stumbles, dropping stolen loot.",
            "The bandit spits and takes your coin purse."
        ),
        (
            "Fire Sprite",
            new() { "hurls a spark!", "ignites the air around you!" },
            "A flickering fire sprite dances into view!",
            "The sprite flickers out with a hiss.",
            "The sprite twirls in glee as you burn."
        ),
        (
            "Stone Golem",
            new() { "smashes with a heavy fist!", "stomps the earth!" },
            "The ground rumbles as a stone golem awakens!",
            "The golem cracks and collapses into rubble.",
            "The golem stands silent as your body falls still."
        )
    };

    public static Enemy CreateRandom(List<(string name, List<string> attacks, string encounter, string death, string win)> templates)
    {
        var (name, attacks, encounter, death, win) = templates[rand.Next(templates.Count)];

        int hp = rand.Next(10, 80);
        int mana = rand.Next(0, 10);
        int evasion = rand.Next(-5, 10);
        int minDmg = rand.Next(1, 5);
        int maxDmg = rand.Next(minDmg + 1, minDmg + 5);
    
        // New random stats
        int dodgeChance = rand.Next(0, 5);          // 3-7% dodge chance
        int physicalDefense = rand.Next(-10, 10);     // 5-10 physical defense
        int criticalHitChance = rand.Next(0, 6);    // 3-5% crit chance

        return new Enemy(
            name, 
            hp, 
            mana, 
            evasion, 
            minDmg, 
            maxDmg, 
            attacks, 
            encounter, 
            death, 
            win,
            dodgeChance,
            physicalDefense,
            criticalHitChance
        );
    }
    
    public static Enemy CreateSpecific(string name)
    {
        var enemy = templates.FirstOrDefault(e => e.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (enemy == default)
            throw new ArgumentException($"Enemy not found: {name}");
        
        int hp = rand.Next(10, 20);
        int mana = rand.Next(0, 10);
        int evasion = rand.Next(0, 10);
        int minDmg = rand.Next(1, 3);
        int maxDmg = rand.Next(minDmg + 1, minDmg + 5);
        
        return new Enemy(name, hp, mana, evasion, minDmg, maxDmg, enemy.attacks, enemy.encounter, enemy.death, enemy.win);
    }
    
}
