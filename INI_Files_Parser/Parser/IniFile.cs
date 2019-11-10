using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace INI_Files_Parser.Parser
{
    public class IniFile
    {
        // private
        private string _iniFileFilename;

        private List<IniSection> iniContent;

        private IniSection GetSection(string section)
        {
            foreach (var aSection in iniContent)
            {
                if (section.ToUpper() == aSection.sectionName.ToUpper())
                {
                    return aSection;
                }
            }
            return null;
        }

        private void _Load()
        {
            if (!File.Exists(_iniFileFilename))
            {
                return;
            }

            string[] lines = File.ReadAllLines(_iniFileFilename, Encoding.GetEncoding("iso-8859-1"));
            IniSection section = null;
            
            foreach (string line in lines)
            {
                if ((line != "") && (line.Substring(0, 1) != ";"))
                {                
                    // If new section
                    if (line.Substring(0, 1) == "[")
                    {
                        string sectionName = line.Substring(1, line.Length - 2);
                        section = new IniSection(sectionName);
                        iniContent.Add(section);
                    }
                    else
                    {
                        // Treat as value, if not empty
                        string[] strList = line.Split('='); // use the ' to define a char
                        string key = strList[0].Replace(" ", "");
                        string value = strList[1].Split(';')[0].Replace(" ", "");
                        if (strList.Length > 2)
                        {
                            for (int i = 2; i < strList.Length; i++)
                            {
                                value = value + "=" + strList[i].Split(';')[0].Replace(" ", "");
                            }
                        }

                        section?.Add(key, value);
                    }
                }
            }
        }
        // constructor
        
        /// <summary>
        /// Create assigns the iniFileName parameter to the FileName property.
        /// </summary>
        /// <param name="iniFilename">Specify the filename of the ini file to use (loaded if it exists)</param>
        public IniFile(string iniFilename)
        {
            _iniFileFilename = iniFilename;
            iniContent = new List<IniSection>();
            _Load();
        }

        // public
        
        /// <summary>
        /// Contains the filename of the ini file from which to read and write information. 
        /// </summary>
        public string fileName
        {
            get { return _iniFileFilename; }
            set { _iniFileFilename = value; }
        }
        
        /// <summary>
        /// Get the IniSection object that holds section information.
        /// </summary>
        /// <param name="sectionName">Name of an INI file section</param>
        /// <returns></returns>
        public IniSection ReadSection(string sectionName)
        {
            return GetSection(sectionName);
        }
        
        /// Call ReadSections to retrieve the names of all sections in an INI file into a List of string. 
        public List<string> ReadSections()
        {
            var result = new List<string>();

            foreach (var section in iniContent)
            {
                result.Add(section.sectionName);
            }

            return result;
        }
        
        /// Call ReadSectionKeys to read the keys, within a specified section of an INI file into a List of string.
        /// <param name="sectionName">Name of an INI file section</param>
        public List<string> ReadSectionKeys(string sectionName)
        {
            var section = GetSection(sectionName);
            if (section == null)
            {
                // return empty list if section wasn't found
                return new List<string>();
            }

            return section.ReadSectionKeys();
        }

        /// <summary>
        /// Call ReadString to read a string value from an INI file.
        /// </summary>
        /// <param name="sectionName">sectionName identifies the section in the file that contains the desired key.</param>
        /// <param name="keyName">keyName is the name of the key from which to retrieve the value.</param>
        /// <param name="defaultValue">defaultValue is the string value to return if the section or key does not exists</param>
        /// <returns></returns>
        public string ReadString(string sectionName, string keyName, string defaultValue)
        {
            var section = GetSection(sectionName);
            if (section == null)
            {
                return defaultValue;
            }

            string result;
            if (!section.TryGetValue(keyName, out result))
            {
                result = defaultValue;
            }
            return result;
        }
        

        /// <summary>
        /// Loads the Inifile specified in filename.
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (!File.Exists(fileName))
            {
                return false;
            }

            _Load();
            return true;
        }

        /// <summary>
        /// Use SectionExists to determine whether a section exists within the ini file specified in FileName. SectionExists returns a Boolean value that indicates whether the section in question exists.
        /// </summary>
        /// <param name="sectionName">sectionName is the ini file section SectionExists determines the existence of.</param>
        /// <returns></returns>
        public bool SectionExists(string sectionName)
        {
            foreach (var section in iniContent)
            {
                if (sectionName.ToUpper() == section.sectionName.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Use KeyExists to determine whether a key exists in the ini file specified in FileName. ValueExists returns a boolean value that indicates whether the key exists in the specified section.
        /// </summary>
        /// <param name="sectionName">sectionName is the section in the ini file in which to search for the key.</param>
        /// <param name="keyName">keyName is the name of the key to search for.</param>
        /// <returns></returns>
        public bool KeyExists(string sectionName, string keyName)
        {
            foreach (var section in iniContent)
            {
                if (section.sectionName.ToUpper() == sectionName.ToUpper())
                {
                    if (section.ContainsKey(keyName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Call ReadInteger to read a integer (int) value from an INI file.
        /// </summary>
        /// <param name="sectionName">sectionName identifies the section in the file that contains the desired key.</param>
        /// <param name="keyName">keyName is the name of the key from which to retrieve the value.</param>
        /// <param name="defaultValue">defaultValue is the integer (int) value to return if the section or key does not exists</param>
        /// <returns></returns>
        public int ReadInteger(string sectionName, string keyName, int defaultValue)
        {
            if (!KeyExists(sectionName, keyName))
            {
                return defaultValue;
            }

            int result = Convert.ToInt32(ReadString(sectionName, keyName, "-1"));
            return result;
        }

        /// <summary>
        /// Call ReadInteger64 to read a 64bit integer (long) value from an INI file.
        /// </summary>
        /// <param name="sectionName">sectionName identifies the section in the file that contains the desired key.</param>
        /// <param name="keyName">keyName is the name of the key from which to retrieve the value.</param>
        /// <param name="defaultValue">defaultValue is the 64bit integer (long) value to return if the section or key does not exists</param>
        /// <returns></returns>
        public long ReadInteger64(string sectionName, string keyName, long defaultValue)
        {
            if (!KeyExists(sectionName, keyName))
            {
                return defaultValue;
            }

            long result = Convert.ToInt64(ReadString(sectionName, keyName, "-1"));
            return result;
        }

        /// <summary>
        /// Call ReadFloat to read a double value from an INI file.
        /// </summary>
        /// <param name="sectionName">sectionName identifies the section in the file that contains the desired key.</param>
        /// <param name="keyName">keyName is the name of the key from which to retrieve the value.</param>
        /// <param name="defaultValue">defaultValue is the double value to return if the section or key does not exists</param>
        /// <returns></returns>
        public double ReadFloat(string sectionName, string keyName, double defaultValue)
        {
            if (!KeyExists(sectionName, keyName))
            {
                return defaultValue;
            }

            double result = double.Parse(ReadString(sectionName, keyName, "0"));
            return result;
        }
    }
}