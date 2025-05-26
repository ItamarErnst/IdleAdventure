

namespace IdleAdventure.Areas
{
    public class EventBuilder
    {
        private string _description = string.Empty;
        private Action<Character>? _action;
        private List<AdventureEvent> _followUps = new();
        private List<AdventureEvent> _successFollowUps = new();
        private List<AdventureEvent> _failFollowUps = new();
        private string? _areaTransition;
        private double? _transitionChance;

        public class SkipEventExecution : Exception {}

        public static EventBuilder Describe(string description)
        {
            return new EventBuilder { _description = description };
        }
        
        public EventBuilder WithAction(Action<Character> action)
        {
            _action = action;
            return this;
        }

        public EventBuilder WithTransition(string area, double chance = 1.0)
        {
            _transitionChance = chance;
            _action = (c) =>
            {
                if (Random.Shared.NextDouble() <= chance)
                {
                    c.CurrentArea = area;
                }
                else
                {
                    Thread.Sleep(GlobalTimer.EventTimer);
                    ColorText.WriteLine("-- You decided to ignore it.", ConsoleColor.DarkGray);
                }
            };
            return this;
        }

        public EventBuilder WithChanceOutcome(double chance, AdventureEvent[] successFollowUps, AdventureEvent[] failFollowUps)
        {
            _action = (c) =>
            {
                bool success = Random.Shared.NextDouble() < chance;
                Thread.Sleep(GlobalTimer.TurnTimer);
                ColorText.WriteLine(success ? "You go for it..." : "You hesitate...", ConsoleColor.Gray);

                var chosen = success ? successFollowUps : failFollowUps;
                if (chosen.Length > 0)
                {
                    var next = chosen[Random.Shared.Next(chosen.Length)];
                    next.Execute(c);
                }
            };
            return this;
        }

        public EventBuilder WithFollowUps(params AdventureEvent[] events)
        {
            _followUps.AddRange(events);
            return this;
        }

        public EventBuilder AsCombat(Enemy enemy, AdventureEvent? onWin = null, AdventureEvent? onLose = null)
        {
            _action = (character) =>
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
            };
            return this;
        }


        public AdventureEvent Build()
        {
            var evt = new AdventureEvent(_description, _action, _areaTransition);
            evt.FollowUps.AddRange(_followUps);
            return evt;
        }
    }
}
