

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
                CombatSystem.Run(character, enemy, onWin, onLose);
            };
            return this;
        }
        
        public AdventureEvent Build()
        {
            var evt = new AdventureEvent(_description, _action, _areaTransition);
            return evt;
        }
    }
}
