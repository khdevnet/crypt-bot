using Crypto.Bot.Domain.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crypto.Bot.Domain.Clients
{
    public class CoingeckoClient
    {
        private readonly HttpClient httpClient;

        public CoingeckoClient(HttpClient httpClient)
        {
            //https://api.coingecko.com/api/v3/coins/list?include_platform=true
            httpClient.BaseAddress = new Uri("https://api.coingecko.com/api/");
            this.httpClient = httpClient;
        }

        public async Task<List<Coin>> GetCoinsAsync()
        {
            var response = await httpClient.GetAsync("v3/coins/list?include_platform=true");

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Coin>>(responseStream);
            return Map(items);

        }

        public async Task<CoinDetails> GetCoinAsync(string id)
        {
            var response = await httpClient.GetAsync($"v3/coins/{id}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false");

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<CoinDetails>(responseStream);
            return Map(item);

        }


        //v3/coins/berry-data?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false

        private static List<Coin> Map(List<Coin> items)
        {
            return items.Select(x => Map(x)).ToList();
        }

        private static Coin Map(Coin x)
        {
            if (x == null) return null;

            return new Coin
            {
                Id = x.Id,
                Name = x.Name,
                Symbol = x.Symbol,
                Platforms = x.Platforms.ContainsKey("") ? null : x.Platforms
            };
        }

        private static CoinDetails Map(CoinDetails x)
        {
            if (x == null) return null;

            var c = new CoinDetails
            {
                Id = x.Id,
                Name = x.Name,
                Symbol = x.Symbol,
                Platforms = x.Platforms.ContainsKey("") ? null : x.Platforms
            };

            if(x.Market_data?.Current_price == null)
            {
                return c;
            }


            c.Market_data = new MarketData { Current_price = x.Market_data.Current_price.ContainsKey("") ? null : x.Market_data.Current_price }; 

            return c;
                 
        }
    }
}
