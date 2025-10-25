using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace SevenVpnApi
{
    public class SevenVpn
    {
        private readonly HttpClient httpClient;
        private readonly long key = 1523615231263112;
        private readonly string apiUrl = "https://panel.7vpn.com/api.cgi";
        private readonly string staticApiUrl = "https://static.7vpn.com";
        private string userId;
        private string securityIdentifier;
        public SevenVpn()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("7vpn/3.4.0 (8830; Android)");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("key", key.ToString());
        }

        public async Task<string> Register(string login, string password)
        {
            var data = JsonContent.Create(new { login = login, email = login, password = password });
            var response = await httpClient.PostAsync($"{apiUrl}/user/registration", data);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Login(string login, string password)
        {
            var data = JsonContent.Create(new { login = login, password = password });
            try
            {
                var response = await httpClient.PostAsync($"{apiUrl}/users/login", data);
                var responseContent = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseContent);
                if (doc.RootElement.TryGetProperty("sid", out var sidElement))
                {
                    securityIdentifier = sidElement.GetString();
                    httpClient.DefaultRequestHeaders.Add("usersid", securityIdentifier);
                }
                if (doc.RootElement.TryGetProperty("uid", out var uidElement))
                {
                    userId = uidElement.GetString();
                }
                return responseContent;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public async Task<string> GetAccountInfo()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/user");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountInternet()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/user/internet");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetServers()
        {
            var response = await httpClient.GetAsync($"{staticApiUrl}/serverlistplan.json");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
