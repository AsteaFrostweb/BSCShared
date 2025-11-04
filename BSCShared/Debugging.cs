using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BSCShared
{
    public class Debugging
    {
        //
        public static HashSet<string> whitelist = new HashSet<string>() { "Server", "Program", "ClientSessionHandler" }; 
        public static void Log(string identifier, string s) 
        {
            if(whitelist.Contains(identifier))
                Console.WriteLine(GetTimeStamp() + " " + s);
        }
        public static void LogError(string identifier, string s)
        {
            if (whitelist.Contains(identifier))
                Console.WriteLine(GetTimeStamp() + " " + "ERROR: " + s);
        }


        public static string GetTimeStamp() 
        {
            return DateTime.Now.ToString("[HH:mm:ss]");
        }
    }
}
