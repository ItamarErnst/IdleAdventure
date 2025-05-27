using System;
using System.Collections.Generic;
using IdleAdventure.AreaFactories;

namespace IdleAdventure.Areas
{
    public static class AreaRegistry
    {
        private static readonly Dictionary<string, Func<Area>> _areaFactories = new()
        {
            { "DarkCave", AreaFactory_Cave.Create },
            { "MeadowField", AreaFactory_Meadow.Create },
            { "ForestShrine", AreaFactory_ForestShrine.Create },
            { "ForgottenCrypt", AreaFactory_ForgottenCrypt.Create },
            { "Village", AreaFactory_Village.Create }
        };

        public static Area Get(string areaName)
        {
            if (_areaFactories.TryGetValue(areaName, out var factory))
                return factory();

            throw new ArgumentException($"Unknown area: {areaName}");
        }

        public static IEnumerable<string> AllAreaNames => _areaFactories.Keys;
    }
}