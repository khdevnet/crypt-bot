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
    public class PriceAlertBotCommand : IBotCommand
    {
        private const string AddCommandText = "add ";
        private const string RemoveCommandText = "rmv ";

        private readonly TelegramBotClient bot;
        private readonly EntityRepository<PriceAlert> alertRepository;

        private string AddCmd => $"{Name} {AddCommandText}";
        private string RemoveCmd => $"{Name} {RemoveCommandText}";

        public string Name { get; } = BotCommands.PriceAlert;

        public PriceAlertBotCommand(TelegramBotClient bot, EntityRepository<PriceAlert> alertRepository)
        {
            this.bot = bot;
            this.alertRepository = alertRepository;
        }

        public async Task Execute(Message message)
        {
            // /pa add id=berry-data&c=usd&t=drops(rises)&p=10
            if (message.Text.StartsWith(AddCmd))
            {
                await AddAsync(message);
                return;
            }

            // /pa rmv id=berry-data
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

            var queryArgs = commandArgs[0];
            var alert = ParceAlert(queryArgs);

            var alerts = alertRepository.GetAll(message.Chat.Id);

            if (!alerts.Any(l => l.Name == alert.Name))
            {
                alert.ChatId = message.Chat.Id;
                await alertRepository.InsertAsync(new[] { alert });
            }
        }

        private async Task RemoveAsync(Message message)
        {
            var commandArgs = message.Text.Split(new[] { RemoveCmd }, StringSplitOptions.RemoveEmptyEntries);
            if (commandArgs.Length == 0)
            {
                return;
            }

            var queryArgs = commandArgs[0];
            var query = HttpUtility.ParseQueryString(queryArgs);

            var name = query.Get("id");
            var alerts = alertRepository.GetAll(message.Chat.Id);

            if (alerts.Any(l => l.Name == name))
            {
                await alertRepository.DeleteAsync(message.Chat.Id, new[] { name });
            }
        }

        private static PriceAlert ParceAlert(string queryArgs)
        {
            var query = HttpUtility.ParseQueryString(queryArgs);

            var targetPrice = double.Parse(query.Get("p"));
            var name = query.Get("id");
            var currency = query.Get("c");
            var type = (PriceAlertType)Enum.Parse(typeof(PriceAlertType), query.Get("t"), true);

            return new PriceAlert { Name = name, Type = type, Currency = currency, Price = targetPrice };
        }
    }
}
