using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace INI_Files_Parser.Parser
{
    public class IniFile
    {
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
            /*if (!File.Exists(_iniFileFilename))
            {
                return;
            }*/
            string[] lines = File.ReadAllLines(_iniFileFilename, Encoding.GetEncoding("iso-8859-1"));
            IniSection section = null;
            foreach (string line in lines)
            {
                if ((line != "") && (line.Substring(0, 1) != ";")) {
                   
                    if (line.Substring(0, 1) == "[")
                    {
                        string sectionName = line.Substring(1, line.Length - 2);
                        section = new IniSection(sectionName);
                        iniContent.Add(section);
                    }else
                    { 
                        try
                        {
                            string[] strList = line.Split('='); 
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
                        catch(IndexOutOfRangeException e)
                        {
                            Console.WriteLine("Cannot covert this line " + line);
                            continue;
                        }
                    }
                }
            }
        }
        public IniFile(string iniFilename)
        {
            _iniFileFilename = iniFilename;
            iniContent = new List<IniSection>();
            _Load();
        }
        
        public string fileName
        {
            get { return _iniFileFilename; }
            set { _iniFileFilename = value; }
        }
        
        public IniSection ReadSection(string sectionName)
        {
            return GetSection(sectionName);
        }
        
        public List<string> ReadSections()
        {
            var result = new List<string>();

            foreach (var section in iniContent)
            {
                result.Add(section.sectionName);
            }

            return result;
        }
        
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
        public string ReadString(string sectionName, string keyName, string defaultValue)
        {
            var section = GetSection(sectionName);
            if (section == null)
            {
                Console.WriteLine("Can't find Section or Name");
                return defaultValue;
            }

            string result;
            if (!section.TryGetValue(keyName, out result))
            {
                result = defaultValue;
            }
            return result;
        }
        
        public bool Load()
        {
            if (!File.Exists(fileName))
            {
                return false;
            }

            _Load();
            return true;
        }
        
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
        
        public int ReadInteger(string sectionName, string keyName, int defaultValue)
        {
            if (!KeyExists(sectionName, keyName))
            {
                Console.WriteLine("Can't find Section or Name");
                return defaultValue;
            }

            int result;
            bool flag = Int32.TryParse(ReadString(sectionName, keyName, "-1"), out result);
            if (flag)
            {
                return result;
            }
            else
            {
                Console.WriteLine("Can not convert to Int");
                return result;
            }
        }
        
        public double ReadFloat(string sectionName, string keyName, double defaultValue)
        {
            if (!KeyExists(sectionName, keyName))
            {
                Console.WriteLine("Can't find Section or Name");
                return defaultValue;
            }

            double result;            
            bool flag = double.TryParse(ReadString(sectionName, keyName, "0,0"), out result);
            if (flag)
            {
                return result;
            }
            else
            {
                Console.WriteLine("Can not convert to double");
                return result;
            }
        }
    }
}