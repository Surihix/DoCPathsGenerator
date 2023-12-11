using System.IO;

namespace DoCPathsGenerator
{
    internal class PathStructures
    {
        public static readonly string EventSceneDir = Path.Combine("data", "event", "scene");

        public static readonly string EventLocaleDir = Path.Combine("data", "event", "locale");

        public static readonly string ZoneDir = Path.Combine("data", "zone");

        public static readonly string ZoneLocaleDir = Path.Combine("data", "zone", "locale");
    }
}