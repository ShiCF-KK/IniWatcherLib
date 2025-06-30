using System.Collections.Generic;
using System.IO;

namespace IniWatcherLib
{
    internal static class IniParser
    {
        public static IniFile Parse(string content)
        {
            var ini = new IniFile();
            var reader = new StringReader(content);
            string line;
            string currentSection = "";

            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    //currentSection = line[1..^1].Trim();
                    currentSection = line.Substring(1, line.Length - 2).Trim(); // ✅ 兼容 C# 7.3
                    if (!ini.Sections.ContainsKey(currentSection))
                        ini.Sections[currentSection] = new Dictionary<string, string>();
                }
                else
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        ini.Sections[currentSection][key] = value;
                    }
                }
            }

            return ini;
        }
    }
}
