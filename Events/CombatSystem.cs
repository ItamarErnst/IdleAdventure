using IdleAdventure;

public static class CombatSystem
{
    private record CombatResult(int Damage, bool IsCritical);

    public static void Run(
        Character character,
        Func<Enemy> enemyFactory,
        AdventureEvent? onWin = null,
        AdventureEvent? onLose = null)
    {
        var rand = Random.Shared;
        var enemy = enemyFactory();

        int turn = 1;

        ColorText.WriteLine($"{Colors.Bold}{enemy.EncounterText}", Colors.Description);
        Thread.Sleep(GlobalTimer.EventTimer);
        ColorText.WriteLine($"Combat starts with {Colors.Bold}{enemy.Name}!",Colors.Damage);

        while (enemy.HP > 0 && character.CurrentHP > 0)
        {
            // 🔹 Player attack
            Thread.Sleep(GlobalTimer.TurnTimer);
            PlayerTurn(enemy,character,rand);            
            if (enemy.HP <= 0) break;

            // 🔹 Enemy attack
            Thread.Sleep(GlobalTimer.TurnTimer);

            // Check if player evades based on stats
            EnemyTurn(enemy,character,rand);            
            turn++;
        }

        Thread.Sleep(GlobalTimer.TurnTimer / 2);
        if (character.CurrentHP <= 0)
        {
            onLose?.Execute(character);
        }
        else
        {
            // Calculate experience and loot with luck bonuses
            onWin?.Execute(character);
            HandleCombatRewards(character, enemy);
        }
    }

    private static void PlayerTurn(Enemy enemy, Character character, Random rand)
    {
        if (CheckPotions(character))
        {
            return;
        }
        
        var combatResult = CalculatePlayerDamage(character, enemy);
        
        if (EnemyEvadesAttack(enemy, character,combatResult.IsCritical))
        {
            return;
        }

        // Calculate damage with all bonuses
        string attackDesc = character.GetEquippedWeapon().GetRandomAttackDescription(rand);
        string textColor = combatResult.IsCritical ? Colors.Critical : Colors.Player;

        // Display combat text
        if (combatResult.IsCritical)
        {
            ColorText.Write($"{Colors.Italic}Critical! ", Colors.Critical);
        }

        ColorText.Write($"You {attackDesc} with {character.GetEquippedWeapon().Name} ", textColor);
        ColorText.Write($"{combatResult.Damage}", Colors.Damage);
        ColorText.WriteLine($" dmg.🐾 {enemy.Name} HP: {Math.Max(enemy.HP - combatResult.Damage, 0)}", Colors.HP);

        enemy.HP -= combatResult.Damage;
    }

    private static void EnemyTurn(Enemy enemy, Character character, Random rand)
    {
        if (PlayerEvadesAttack(character))
        {
            ColorText.WriteLine($"You dodge the {enemy.Name}'s attack!", Colors.Player);
            return;
        }

        // Calculate enemy damage with defense reduction
        var enemyDamage = CalculateEnemyDamage(enemy, character);
        string enemyAttack = enemy.GetRandomAttackDescription(rand);
        character.CurrentHP -= enemyDamage;

        ColorText.Write($"{enemy.Name} {enemyAttack} ", Colors.Description);
        ColorText.Write($"{enemyDamage}", Colors.Damage);
        ColorText.WriteLine($" dmg.❤ Your HP: {Math.Max(character.CurrentHP, 0)}", Colors.HP);

    }
    private static bool EnemyEvadesAttack(Enemy enemy, Character character,bool isCritical)
    {
        var multiplyer = isCritical? 0.5f : 1;
        var weaponMissChance = character.GetEquippedWeapon().MissChance * multiplyer;
        var totalEvasionChance = (enemy.Evasion + (enemy?.DodgeChance ?? 0)) * multiplyer;
        
        if (Random.Shared.NextDouble() < weaponMissChance)
        {
            ColorText.WriteLine($"You missed your attack!", Colors.Ignore);
            return true;           
        }

        if (Random.Shared.Next(100) < totalEvasionChance)
        {
            ColorText.WriteLine($"{enemy?.Name} evaded your attack!", Colors.Ignore);
            return true;
        }
        
        return false;
    }

    private static bool PlayerEvadesAttack(Character character)
    {
        return Random.Shared.Next(100) < character.Stats.DodgeChance;
    }


    private static CombatResult CalculatePlayerDamage(Character character, Enemy enemy)
    {
        var weapon = character.GetEquippedWeapon();
        bool isCritical = character.Stats.IsCriticalHit();
        
        // Get base weapon damage
        int baseDamage = Random.Shared.Next(weapon.MinDamage, weapon.MaxDamage + 1);
        
        // Add physical damage bonus
        int totalDamage = baseDamage + character.Stats.PhysicalDamageBonus;
        
        // Apply critical hit multiplier and bonus
        if (isCritical)
        {
            totalDamage = (int)(totalDamage * 1.5f);
            totalDamage += character.Stats.CriticalSynergyBonus;
        }
        
        // Add strength/endurance synergy bonus
        totalDamage += character.Stats.PhysicalSynergyBonus;
        
        // Apply enemy defense if available
        totalDamage = (int)(totalDamage * (1 - (enemy.PhysicalDefense / 100f)));
        
        return new CombatResult(Math.Max(1, totalDamage), isCritical);
    }

    private static int CalculateEnemyDamage(Enemy enemy, Character character)
    {
        int baseDamage = enemy.GetDamage(Random.Shared);
        
        // Apply character's physical defense
        float damageReduction = character.Stats.PhysicalDefense / 100f;
        int finalDamage = (int)(baseDamage * (1 - damageReduction));
        
        return Math.Max(1, finalDamage);
    }

    private static void HandleCombatRewards(Character character, Enemy enemy)
    {
        // Base XP modified by luck
        float luckBonus = 1 + (character.Luck * 0.02f);
        int xpGained = (int)(enemy.XPValue * luckBonus);
        
        // Generate loot with luck-based chances
        float dropChance = 0.1f * character.Stats.LootChanceModifier();
        
        character.Inventory.AddGold(Random.Shared.Next(0, 6));
        character.GainXP(xpGained);
        
        if (Random.Shared.NextDouble() < dropChance)
        {
            character.Inventory.AddItem(Random.Shared.Next(0, 3) == 0 ? "Magic Potion" : "Health Potion");
            if (Random.Shared.NextDouble() < 0.5)
            {
                character.Inventory.AddWeapon(WeaponFactory.CreateRandom());
            }
        }
    }

    private static bool CheckPotions(Character character)
    {
        if (character.CurrentHP > character.MaxHP / 0.5f)
        {
            if (character.Inventory.HasItem("Health Potion"))
            {
                character.Inventory.RemoveItem("Health Potion");
                ColorText.Write($"You took out a Health Potion and drank it all swiftly", Colors.Player);
                character.HealHP(25);
                return true;
            }
        }

        return false;
    }
}