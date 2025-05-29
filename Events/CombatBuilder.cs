using IdleAdventure.Areas;
using IdleAdventure;
using IdleAdventure.Enemy;

public class CombatBuilder
{
    private List<(string name, List<string> attacks, string encounter, string death, string win)> _enemies = AreaEnemies.meadow;
    private Action<Character>? _onWinAction;
    private Action<Character>? _onLoseAction;
    private readonly List<AdventureEvent> _extraWinEvents = new();

    public CombatBuilder SetEnemies(
        List<(string name, List<string> attacks, string encounter, string death, string win)> enemies)
    {
        _enemies = enemies;
        return this;
    }
    
    public CombatBuilder OnWin(Action<Character> winAction)
    {
        _onWinAction = winAction;
        return this;
    }

    public CombatBuilder OnLose(Action<Character> loseAction)
    {
        _onLoseAction = loseAction;
        return this;
    }

    public CombatBuilder WithRareDrop(AdventureEvent rareEvent)
    {
        if (rareEvent != null)
            _extraWinEvents.Add(rareEvent);
        return this;
    }


    public AdventureEvent Build()
    {
        return EventBuilder
            .FromAction(character =>
            {
                var enemy = EnemyFactory.CreateRandom(_enemies);

                var onWin = new AdventureEvent(enemy.DeathText,
                    _onWinAction ?? (_ => { }),
                    descriptionColor: Colors.HP
                );

                if (_extraWinEvents.Count != 0)
                {
                    foreach (var evt in _extraWinEvents)
                    {
                        onWin.AddNext(evt);
                    }
                }

                var onLose = new AdventureEvent(enemy.WinText,
                    _onLoseAction ?? (_ => { }),
                    descriptionColor: Colors.Item
                );

                CombatSystem.Run(character, () => enemy, onWin, onLose);
            })
            .Build();
    }
}