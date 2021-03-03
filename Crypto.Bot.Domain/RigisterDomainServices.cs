using Crypto.Bot.Domain.Abstraction;
using Crypto.Bot.Domain.Clients;
using Crypto.Bot.Domain.Command;
using Crypto.Bot.Domain.Database;
using Crypto.Bot.Domain.Entity;
using Crypto.Bot.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;

namespace Crypto.Bot.Domain
{
    public static class RigisterDomainServices
    {
        public static void RegisterDomainService(this IServiceCollection services)
        {
            RegisterHttpClients(services);

            RegisterMongoDb(services);

            RegisterTelegramBot(services);
        }

        public static void RegisterMongoDb(this IServiceCollection services)
        {
            DbBsonMapping.MapEntities();

            services.AddSingleton(typeof(MongoClient), (provider) => new MongoClient(provider.GetService<IConfiguration>().GetValue<string>("database:connectionString")));

            services.AddTransient(p => new EntityRepository<Listing>(p.GetService<MongoClient>(), "listings"));
            services.AddTransient(p => new EntityRepository<PriceAlert>(p.GetService<MongoClient>(), "priceAlerts"));
        }

        public static void RegisterTelegramBot(this IServiceCollection services)
        {
            services.AddSingleton(typeof(TelegramBotClient), (provider) => new TelegramBotClient(provider.GetService<IConfiguration>().GetValue<string>("bot:token")));

            services.AddTransient<IBotCommand, ListingBotCommand>();
            services.AddTransient<IBotCommand, PriceAlertBotCommand>();
            services.AddTransient<IBotCommand, DefaultBotCommand>();
        }

        public static void RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddHttpClient<BotWebApiClient>();
            services.AddHttpClient<CoingeckoClient>();
        }
    }
}
