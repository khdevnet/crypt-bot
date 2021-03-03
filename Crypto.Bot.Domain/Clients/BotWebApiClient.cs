using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crypto.Bot.Domain.Clients
{
    public class BotWebApiClient
    {
        private readonly HttpClient httpClient;

        public BotWebApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration.GetValue<string>("botApi:healthCheck")+"/api/");
            this.httpClient = httpClient;
        }

        public async Task<bool> HealthCheck()
        {
            var response = await httpClient.GetAsync("health");

            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

    }
}
