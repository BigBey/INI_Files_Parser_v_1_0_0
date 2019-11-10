using INI_Files_Parser.Parser;

namespace INI_Files_Parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IniFile iniFile = new IniFile("C:\\Users\\zenbook\\RiderProjects\\INI_Files_Parser\\INI_Files_Parser\\Test\\test.ini");
            iniFile.ReadSection("GeneralConfiguration");
        }
    }
}