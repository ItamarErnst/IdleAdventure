namespace IdleAdventure.Areas
{
    public class AreaRegistry
    {
        private readonly Dictionary<string, IAreaFactory> _factories;

        // ✅ Define valid starting and recovery area codes
        private static readonly HashSet<string> StartingAreas = new()
        {
            "MeadowField",
            "Graveyard",
            "RoyalMansion",
            "Shore"
        };

        private static readonly HashSet<string> RecoveryAreas = new()
        {
            "MeadowField",
            "Village",
            "ForestShrine"
        };

        private readonly Random rand = Random.Shared;

        public AreaRegistry()
        {
            _factories = DiscoverFactories();
        }

        private Dictionary<string, IAreaFactory> DiscoverFactories()
        {
            var factoryType = typeof(IAreaFactory);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); } catch { return Array.Empty<Type>(); }
                })
                .Where(t => !t.IsAbstract && factoryType.IsAssignableFrom(t))
                .ToList();

            var dict = new Dictionary<string, IAreaFactory>();

            foreach (var type in types)
            {
                try
                {
                    var instance = (IAreaFactory)Activator.CreateInstance(type)!;
                    var area = instance.Create();
                    dict[area.CodeName] = instance;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load area from factory '{type.Name}': {ex.Message}");
                }
            }

            return dict;
        }

        public Area Get(string areaName)
        {
            if (_factories.TryGetValue(areaName, out var factory))
                return factory.Create();

            throw new ArgumentException($"Unknown area: {areaName}");
        }

        public IEnumerable<string> AllAreaNames => _factories.Keys;

        // ✅ Random starting area
        public string GetRandomStartingArea()
        {
            var options = _factories.Keys.Where(name => StartingAreas.Contains(name)).ToList();
            return options.Count > 0 ? options[rand.Next(options.Count)] : throw new Exception("No valid starting areas found.");
        }

        // ✅ Random recovery area
        public string GetRandomRecoveryArea()
        {
            var options = _factories.Keys.Where(name => RecoveryAreas.Contains(name)).ToList();
            return options.Count > 0 ? options[rand.Next(options.Count)] : throw new Exception("No valid recovery areas found.");
        }
    }
}
