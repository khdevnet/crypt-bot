using System.Collections.Generic;

namespace Crypto.Bot.Domain.Entity
{
    public class Coin
    {
        public string _Id { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public Dictionary<string, string> Platforms { get; set; }
    }

    public class CoinDetails : Coin
    {
        public MarketData Market_data { get; set; }
    }

    public class MarketData
    {
        public Dictionary<string, double> Current_price { get; set; }
    }
}
