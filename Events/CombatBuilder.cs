using IdleAdventure.Areas;
using IdleAdventure;

public class CombatBuilder
{
    private Func<Enemy> _enemyFactory = EnemyFactory.CreateRandom;
    private Action<Character>? _onWinAction;
    private Action<Character>? _onLoseAction;
    private readonly List<AdventureEvent> _extraWinEvents = new();

    public CombatBuilder WithEnemy(Func<Enemy> factory)
    {
        _enemyFactory = factory;
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
                var enemy = _enemyFactory();

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