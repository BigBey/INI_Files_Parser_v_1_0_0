using System.Collections.Generic;

namespace INI_Files_Parser.Parser
{
    public class IniSection : Dictionary<string, string>
    {
        
        internal string sectionName;

        
        internal IniSection(string name)
        {
            sectionName = name;
        }
        
        internal List<string> ReadSectionKeys()
        {
            var result = new List<string>();

            foreach (string key in Keys)
            {
                result.Add(key);
            }

            return result;
        }
    }
}