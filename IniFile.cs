using System.Collections.Generic;

namespace IniWatcherLib
{
    public class IniFile
    {
        //public Dictionary<string, Dictionary<string, string>> Sections { get; } = new();
        public Dictionary<string, Dictionary<string, string>> Sections { get; } = new Dictionary<string, Dictionary<string, string>>();


        public string Get(string section, string key, string defaultValue = null)
        {
            if (Sections.TryGetValue(section, out var kv) && kv.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }

        public static IniFile Parse(string content)
        {
            return IniParser.Parse(content);
        }
    }
}
