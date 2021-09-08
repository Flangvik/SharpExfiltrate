using Newtonsoft.Json;
using SharpExfiltrate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Helpers
{
    public static class O365Helper
    {
        public static async Task<string> GetAccessToken(string username, string password)
        {

            var url = "https://login.windows.net/common/oauth2/token";

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, xcert, chain, errors) =>
                {

                    return true;
                },
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                UseProxy = true,
                PreAuthenticate = true,

            };

            var client = new HttpClient(httpClientHandler);

            var loginPostBody = new FormUrlEncodedContent(new[]
            {
               new KeyValuePair<string, string>("resource", "https://graph.windows.net"),
               new KeyValuePair<string, string>("client_id",  "d3590ed6-52b3-4102-aeff-aad2292ab01c"),
               new KeyValuePair<string, string>("client_info", "1"),
               new KeyValuePair<string, string>("grant_type", "password"),
               new KeyValuePair<string, string>("username", username),
               new KeyValuePair<string, string>("password", password),
               new KeyValuePair<string, string>("scope", "openid")


           });

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            //You might wanna change this :) 
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; WebView/3.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763");
            client.DefaultRequestHeaders.Add("User-Agent", "SharpExfil");


            HttpResponseMessage httpResp = await client.PostAsync(url, loginPostBody);
            string contentResp = await httpResp.Content.ReadAsStringAsync();

            BearerTokenResp tokenResp = null;
            BearerTokenErrorResp errorResp = null;

            if (httpResp.IsSuccessStatusCode)
            {
                tokenResp = JsonConvert.DeserializeObject<BearerTokenResp>(contentResp);
                return tokenResp.refresh_token;
            }
            else
            {
                errorResp = JsonConvert.DeserializeObject<BearerTokenErrorResp>(contentResp);
                return "Bad reponse from o365 => " + errorResp.error_description.Split('\n')[0];
            }



        }

    }
}
