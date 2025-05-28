using IdleAdventure;

public static class CombatSystem
{
    public static void Run(
        Character character,
        Func<Enemy> enemyFactory,
        AdventureEvent? onWin = null,
        AdventureEvent? onLose = null)
    {
        var rand = Random.Shared;
        var enemy = enemyFactory();

        int turn = 1;

        Thread.Sleep(GlobalTimer.EventTimer);
        ColorText.WriteLine(enemy.EncounterText, Colors.Description);
        Thread.Sleep(GlobalTimer.EventTimer);
        ColorText.WriteLine($"Combat starts with {enemy.Name}!",Colors.Description);

        while (enemy.HP > 0 && character.CurrentHP > 0)
        {
            // 🔹 Player attack
            Thread.Sleep(GlobalTimer.TurnTimer);
            bool isCrit;
            int playerDamage = character.GetEquippedWeapon().GetDamage(rand, out isCrit);

            if (rand.NextDouble() < character.GetEquippedWeapon().MissChance || rand.Next(100) < enemy.Evasion)
            {
                ColorText.WriteLine("You miss your attack!", Colors.Ignore);
            }
            else
            {
                string attackDesc = character.GetEquippedWeapon().GetRandomAttackDescription(rand);
                string textColor = isCrit ? Colors.Critical : Colors.Player;

                if (isCrit)
                    ColorText.Write($"{Colors.Italic}Critical! ", Colors.Critical);

                ColorText.Write($"You {attackDesc} with {character.GetEquippedWeapon().Name} ", textColor);
                ColorText.Write($"{playerDamage}", Colors.Damage);
                ColorText.WriteLine($" dmg. 🐾 {enemy.Name} HP: {Math.Max(enemy.HP - playerDamage, 0)}", Colors.HP);

                enemy.HP -= playerDamage;
            }

            if (enemy.HP <= 0) break;

            // 🔹 Enemy attack
            Thread.Sleep(GlobalTimer.TurnTimer);
            int enemyDamage = enemy.GetDamage(rand);

            if (rand.Next(100) < character.Evasion)
            {
                ColorText.WriteLine($"You dodge the {enemy.Name}'s attack!", Colors.Player);
            }
            else
            {
                string enemyAttack = enemy.GetRandomAttackDescription(rand);
                character.CurrentHP -= enemyDamage;

                ColorText.Write($"{enemy.Name} {enemyAttack} ", Colors.Description);
                ColorText.Write($"{enemyDamage}", Colors.Damage);
                ColorText.WriteLine($" dmg. ❤️ Your HP: {Math.Max(character.CurrentHP, 0)}", Colors.HP);
            }

            turn++;
        }

        Thread.Sleep(GlobalTimer.TurnTimer / 2);
        if (character.CurrentHP <= 0)
        {
            onLose?.Execute(character);
        }
        else
        {
            onWin?.Execute(character);
        }
    }
}
