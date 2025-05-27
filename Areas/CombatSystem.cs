namespace IdleAdventure.Areas;

public static class CombatSystem
{
    public static void Run(Character character, Enemy enemy, AdventureEvent? onWin = null, AdventureEvent? onLose = null)
    {
        var rand = Random.Shared;
        int turn = 1;

        Thread.Sleep(GlobalTimer.TurnTimer);
        ColorText.WriteLine($"Combat starts with {enemy.Name}!", ConsoleColor.Red);

        while (enemy.HP > 0 && character.CurrentHP > 0)
        {
            // Player attack
            Thread.Sleep(GlobalTimer.TurnTimer);
            bool isCrit;
            int playerDamage = character.GetEquippedWeapon().GetDamage(rand, out isCrit);

            if (rand.NextDouble() < character.GetEquippedWeapon().MissChance || rand.Next(100) < enemy.Evasion)
            {
                ColorText.WriteLine("You miss your attack!", ConsoleColor.Gray);
            }
            else
            {
                string attackDesc = character.GetEquippedWeapon().GetRandomAttackDescription(rand);
                ConsoleColor textColor = isCrit ? ConsoleColor.Green : ConsoleColor.Yellow;

                if (isCrit)
                    ColorText.Write("Critical! ", ConsoleColor.Green);

                ColorText.Write($"You {attackDesc} with {character.GetEquippedWeapon().Name} ", textColor);
                ColorText.Write($"{playerDamage}", ConsoleColor.Red);
                ColorText.WriteLine($" dmg. 🐾 {enemy.Name} HP: {Math.Max(enemy.HP - playerDamage, 0)}", ConsoleColor.DarkRed);

                enemy.HP -= playerDamage;
            }

            if (enemy.HP <= 0) break;

            // Enemy attack
            Thread.Sleep(GlobalTimer.TurnTimer);
            int enemyDamage = enemy.GetDamage(rand);

            if (rand.Next(100) < character.Evasion)
            {
                ColorText.WriteLine($"You dodge the {enemy.Name}'s attack!", ConsoleColor.Cyan);
            }
            else
            {
                string enemyAttack = enemy.GetRandomAttackDescription(rand);
                character.CurrentHP -= enemyDamage;

                ColorText.Write($"{enemy.Name} {enemyAttack} ", ConsoleColor.White);
                ColorText.Write($"{enemyDamage}", ConsoleColor.Red);
                ColorText.WriteLine($" dmg. ❤️ Your HP: {Math.Max(character.CurrentHP, 0)}", ConsoleColor.DarkRed);
            }

            turn++;
        }

        Thread.Sleep(GlobalTimer.TurnTimer / 2);
        if (character.CurrentHP <= 0)
        {
            ColorText.WriteLine("You have fallen in battle...", ConsoleColor.DarkRed);
            onLose?.Execute(character);
        }
        else
        {
            ColorText.WriteLine($"You defeated {enemy.Name}!", ConsoleColor.Green);
            onWin?.Execute(character);
        }
    }
}
