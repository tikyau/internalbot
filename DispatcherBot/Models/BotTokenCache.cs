using DispatcherBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DispatcherBot.Models
{
    public class BotTokenCache
    {
        /*  Token sample
        {
            "token_type":"Bearer",
            "expires_in":3600,
            "ext_expires_in":3600,
            "access_token":"eyJhbGciOiJIUzI1Ni..."
        }
         * */
        private static Dictionary<string, AccessToken> Token = new Dictionary<string, AccessToken>();
        public static async Task<string> Get(string bot)
        {
            AccessToken token = Token.ContainsKey(bot) ? Token[bot] : null;
            if (token == null)
            {
                AccessToken o = (await BotConnectorAuthenticationService.GetAccessToken(DataSource.RegisteredBotSecrets[bot].Key, DataSource.RegisteredBotSecrets[bot].Value));
                o.absolute_expire = DateTime.UtcNow.AddSeconds((int)o.expires_in);
                Set(bot, o);
                return (string)o.access_token;
            }
            else
            {
                if(((DateTime)token.absolute_expire) >= DateTime.UtcNow.AddMilliseconds(-1))
                {
                    Token.Remove(bot);
                    return await Get(bot);
                }
                else
                {
                    return (string)token.access_token;
                }
            }
        }
        private static void Set(string bot, AccessToken token) { Token[bot] = token; }
    }
}