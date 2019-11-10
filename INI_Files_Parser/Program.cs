using System;
using INI_Files_Parser.Parser;

namespace INI_Files_Parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var ini = new IniFile(@"C:\Users\zenbook\RiderProjects\INI_Files_Parser_v_1_0_0\INI_Files_Parser\Test\test.ini");

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

            long i64 = ini.ReadInteger64("valueTests", "integer64", 0);
            Console.WriteLine("Integer64=" + i64.ToString());

            string s = ini.ReadString("Users", "ricky", "");
            Console.WriteLine("String=" + s);

            

            Console.ReadLine();
        }
    }
}