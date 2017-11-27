using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DispatcherBot.Models
{
    /// <summary>
    /// This is a mock datasource class, you should change below codes to actually retrieve BOT information from your choosen storage
    /// </summary>
    public class DataSource
    {
#if true
        public static Dictionary<string, string> RegisteredBots = new Dictionary<string, string>
        {
            { "IT BOT","ITURL" },
            { "HR BOT","https://<HRWEB URL>.azurewebsites.net/api/messages"},
            { "Main BOT","https://<ROOT BOT URL>.azurewebsites.net/api/messages"}
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
            { "IT BOT", new KeyValuePair<string,string>("{ROOT BOT ID}","ROOT BOT PASSWORD") },
            { "HR BOT", new KeyValuePair<string,string>("{ROOT BOT ID}","{ROOT BOT PASSWORD}") }
        };
    }
}