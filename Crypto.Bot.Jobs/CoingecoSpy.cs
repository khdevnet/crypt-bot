using System.Linq;
using System.Threading.Tasks;
using Crypto.Bot.Domain.Clients;
using Crypto.Bot.Domain.Entity;
using Crypto.Bot.Domain.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Crypto.Bot.Jobs
{
    public class CoingecoSpy
    {
        private readonly CoingeckoClient coingeckoClient;
        private readonly EntityRepository<Listing> listingRepository;
        private readonly EntityRepository<PriceAlert> alertsRepository;
        private readonly TelegramBotClient botClient;

        public CoingecoSpy(
            CoingeckoClient coingeckoClient,
            EntityRepository<Listing> listingRepository,
            EntityRepository<PriceAlert> alertsRepository,
            TelegramBotClient botClient)
        {
            this.coingeckoClient = coingeckoClient;
            this.listingRepository = listingRepository;
            this.alertsRepository = alertsRepository;
            this.botClient = botClient;
        }

        [FunctionName("TrackListing")]
        public async Task TrackListing([TimerTrigger("* */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var newCoins = await coingeckoClient.GetCoinsAsync();
            var listings = await listingRepository.GetAllAsync();

            foreach (var listing in listings)
            {
                if (newCoins.Any(c => c.Name.ToLower().Contains(listing.Name.ToLower())))
                {
                    await botClient.SendTextMessageAsync(chatId: listing.ChatId,
                                                         text: $"Coin listed: {listing.Name}");

                    await listingRepository.DeleteAsync(listing.ChatId, new[] { listing.Name });

                }
            }
        }

        [FunctionName("TrackPriceAlerts")]
        public async Task TrackPriceAlerts([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            var alerts = await alertsRepository.GetAllAsync();

            foreach (var alert in alerts)
            {
                var coin = await coingeckoClient.GetCoinAsync(alert.Name);

                if (coin == null)
                {
                    return;
                }

                if (coin.Market_data.Current_price.TryGetValue(alert.Currency, out var currentPrice))
                {
                    if ((alert.Type == PriceAlertType.Rises && currentPrice > alert.Price) || (alert.Type == PriceAlertType.Drops && currentPrice < alert.Price))
                    {
                        await botClient.SendTextMessageAsync(chatId: alert.ChatId,
                                                         text: $"{alert.Name} price {alert.Type} {alert.Price} {alert.Currency}, Current price is {currentPrice} {alert.Currency}");
                    }
                }
            }
        }
    }
}
