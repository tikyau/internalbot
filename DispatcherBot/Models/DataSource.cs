using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DispatcherBot.Models
{
    public class DataSource
    {
#if true
        public static Dictionary<string, string> RegisteredBots = new Dictionary<string, string>
        {
            { "IT BOT","ITURL" },
            { "HR BOT","https://hrbotdemo001.azurewebsites.net/api/messages"},
            { "Main BOT","https://dispatcherbotdemo.azurewebsites.net/api/messages"}
        };
#else
        public static Dictionary<string, string> RegisteredBots = new Dictionary<string, string>
        {
            { "IT BOT","ITURL" },
            { "HR BOT","http://localhost:10838//api/messages"},
            { "Main BOT","http://lcoalhost:23978/api/messages"}
        };
#endif
        public static Dictionary<string, KeyValuePair<string, string>> RegisteredBotSecrets = new Dictionary<string, KeyValuePair<string, string>>
        {
            { "IT BOT", new KeyValuePair<string,string>("","") },
            { "HR BOT", new KeyValuePair<string,string>("016141c9-bc4d-4524-a299-5d05e878d2cd","gccbiAWLF198!xdVOP94*+~") }
        };
    }
}