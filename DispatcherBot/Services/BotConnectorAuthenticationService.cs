using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DispatcherBot.Services
{
    /*
        {
            "token_type":"Bearer",
            "expires_in":3600,
            "ext_expires_in":3600,
            "access_token":"eyJhbGciOiJIUzI1Ni..."
        }  
     * */
    public class AccessToken
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
        public DateTime absolute_expire { get; set; }
    }
    public class BotConnectorAuthenticationService
    {
        private static async Task<string> RequestAccessTokenFromMSAAADv2(string appId, string appPassword)
        {
            var http = new HttpClient();
            //var payload = new StringContent($"grant_type=client_credentials&client_id={appId}&client_secret={appPassword}&scope=https%3A%2F%2Fapi.botframework.com%2F.default");
            var payload = new StringContent($"grant_type=client_credentials&client_id={appId}&client_secret={appPassword}&scope={appId}%2F.default");
            payload.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var resp = await http.PostAsync(new Uri("https://login.microsoftonline.com/botframework.com/oauth2/v2.0/token"),
                                    payload);
            var body = await resp.Content.ReadAsStringAsync();
            using (var sr = new StreamReader(await resp.Content.ReadAsStreamAsync()))
            {
                body = sr.ReadToEnd();
            }
            return body;
        }

        public static async Task<AccessToken> GetAccessToken(string appId, string appPassword)
        {
            var body = await RequestAccessTokenFromMSAAADv2(appId, appPassword);
            AccessToken resp = JsonConvert.DeserializeObject<AccessToken>(body);
            return resp;
        }
    }
}