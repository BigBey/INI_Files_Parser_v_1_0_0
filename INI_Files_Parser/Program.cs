using System;
using System.IO;
using INI_Files_Parser.Parser;

namespace INI_Files_Parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string path = @"C:\Users\zenbook\RiderProjects\INI_Files_Parser_v_1_0_0\INI_Files_Parser\Test\test.ini";
            try
            {
                if (Path.GetExtension(path) == ".ini")
                {
                    IniFile ini = new IniFile(path);

                    var sections = ini.ReadSections();

                    int cnt = 1;
                    foreach (string sectionName in sections)
                    {
                        Console.WriteLine("#" + cnt.ToString() + " - " + sectionName);

                        var keys = ini.ReadSectionKeys(sectionName);
                        foreach (string keyName in keys)
                        {
                            Console.WriteLine(keyName + " = " + ini.ReadString(sectionName, keyName, "UNKNOWN"));
                        }

                        Console.WriteLine("");

                        cnt++;
                    }

                    int i = ini.ReadInteger("GeneralConfiguration", "setUpdate", 0);
                    Console.WriteLine("Integer=" + i.ToString());

                    string s = ini.ReadString("Users", "ricky", "");
                    Console.WriteLine("String=" + s);

                    double d = ini.ReadFloat("Users", "BufferLenSeconds", 0.0);
                    Console.WriteLine("Double=" + d);

                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Not right format");
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        
    }
}