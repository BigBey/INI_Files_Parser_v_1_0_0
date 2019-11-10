using System.Collections.Generic;

namespace INI_Files_Parser.Parser
{
    public class IniSection : Dictionary<string, string>
    {
        /// <summary>
        /// Name of the section.
        /// </summary>
        public string sectionName;

        /// <summary>
        /// Section name is set to name.
        /// </summary>
        /// <param name="name"></param>
        public IniSection(string name)
        {
            sectionName = name;
        }

        /// <summary>
        /// Call ReadSectionKeys to read the keys into a List of string.
        /// </summary>
        /// <returns></returns>
        public List<string> ReadSectionKeys()
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