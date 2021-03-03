using Crypto.Bot.Domain.Abstraction;
using Crypto.Bot.Domain.Clients;
using Crypto.Bot.Domain.Entity;
using Crypto.Bot.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Crypto.Bot.Domain.Command
{
    public class ListingBotCommand : IBotCommand
    {
        private const string AddCommandText = "add ";
        private const string RemoveCommandText = "rmv ";

        private readonly TelegramBotClient bot;
        private readonly EntityRepository<Listing> listingRepository;
        private readonly CoingeckoClient coingeckoClient;

        private string AddCmd => $"{Name} {AddCommandText}";
        private string RemoveCmd => $"{Name} {RemoveCommandText}";

        public string Name { get; } = BotCommands.Listing;

        public ListingBotCommand(TelegramBotClient bot, EntityRepository<Listing> listingRepository, CoingeckoClient coingeckoClient)
        {
            this.bot = bot;
            this.listingRepository = listingRepository;
            this.coingeckoClient = coingeckoClient;
        }

        public async Task Execute(Message message)
        {
            // /l add n=berry data
            if (message.Text.StartsWith(AddCmd))
            {
                await AddAsync(message);
                return;
            }

            // /l rmv n=berry data
            if (message.Text.StartsWith(RemoveCmd))
            {
                await RemoveAsync(message);
                return;
            }

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Command not exist"
            );
        }

        private async Task AddAsync(Message message)
        {
            var commandArgs = message.Text.Split(new[] { AddCmd }, StringSplitOptions.RemoveEmptyEntries);
            if (commandArgs.Length == 0)
            {
                return;
            }

            var coinName = HttpUtility.ParseQueryString(commandArgs[0]).Get("n");
            var listings = listingRepository.GetAll(message.Chat.Id);

            if (!listings.Any(l => l.Name == coinName))
            {
                await listingRepository.InsertAsync(new[] { new Listing { ChatId = message.Chat.Id, Name = coinName } });
            }
        }

        private async Task RemoveAsync(Message message)
        {
            var commandArgs = message.Text.Split(new[] { RemoveCmd }, StringSplitOptions.RemoveEmptyEntries);
            if (commandArgs.Length == 0)
            {
                return;
            }

            var coinName = HttpUtility.ParseQueryString(commandArgs[0]).Get("n");
            var listings = listingRepository.GetAll(message.Chat.Id);

            if (listings.Any(l => l.Name == coinName))
            {
                await listingRepository.DeleteAsync(message.Chat.Id, new[] { coinName });
            }
        }
    }
}
