using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DispatcherBot.Services
{
    public class MSAOpenIdMetadata
    {
        public string authorization_endpoint { get; set; }
        public string token_endpoint { get; set; }
        public string []token_endpoint_auth_methods_supported { get; set; }
        public string jwks_uri { get; set; }
    }
    public class BotAuthenticationService
    {
        private static async Task<MSAOpenIdMetadata> RequestMSAOpenIdMetadata()
        {
            /*
             * sample response::https://docs.microsoft.com/en-us/bot-framework/rest-api/bot-framework-rest-connector-authentication#emulator-to-bot
            {
                "authorization_endpoint":"https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
                "token_endpoint":"https://login.microsoftonline.com/common/oauth2/v2.0/token",
                "token_endpoint_auth_methods_supported":["client_secret_post","private_key_jwt"],
                "jwks_uri":"https://login.microsoftonline.com/common/discovery/v2.0/keys",
                ...
            }            
                         * */
            //GET https://login.microsoftonline.com/botframework.com/v2.0/.well-known/openid-configuration
            var http = new HttpClient();
            var resp = await http.GetAsync(new Uri("https://login.microsoftonline.com/botframework.com/v2.0/.well-known/openid-configuration"));
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var body = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MSAOpenIdMetadata>(body);
            }
            return null;
        }
        private static async Task<dynamic> GetValidSigningKeys(string jwks_url)
        {
            var http = new HttpClient();
            var resp = await http.GetAsync(new Uri(jwks_url));
            var body = await resp.Content.ReadAsStringAsync();
            return (dynamic)JsonConvert.DeserializeObject<dynamic>(body);
        }
    }
}