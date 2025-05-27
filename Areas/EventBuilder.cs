

namespace IdleAdventure.Areas
{
    public class EventBuilder
    {
        private string _description = string.Empty;
        private Action<Character>? _action;
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

                var chosen = success ? successFollowUps : failFollowUps;
                if (chosen.Length > 0)
                {
                    var next = chosen[Random.Shared.Next(chosen.Length)];
                    next.Execute(c);
                }
            };
            return this;
        }
        
        public AdventureEvent Build()
        {
            var evt = new AdventureEvent(_description, _action);
            return evt;
        }
    }
}
