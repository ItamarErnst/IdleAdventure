

namespace IdleAdventure.Areas
{
    public class EventBuilder
    {
        private string _description = string.Empty;
        private Action<Character>? _action;

        public class SkipEventExecution : Exception {}

        public static EventBuilder Describe(string description)
        {
            return new EventBuilder { _description = description };
        }
        
        public static EventBuilder FromAction(Action<Character> action)
        {
            return new EventBuilder().WithAction(action);
        }
        
        public EventBuilder WithAction(Action<Character> action)
        {
            _action = action;
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
