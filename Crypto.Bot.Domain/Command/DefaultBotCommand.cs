using Crypto.Bot.Domain.Abstraction;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Crypto.Bot.Domain.Command
{
    public class DefaultBotCommand : IBotCommand
    {
        private readonly TelegramBotClient bot;

        public string Name { get; } = BotCommands.Default;

        public DefaultBotCommand(TelegramBotClient bot)
        {
            this.bot = bot;
        }

        public async Task Execute(Message message)
        {
            const string usage = "Usage:\n" +
                                    "/l add n=berry data - listen for new coin by name\n" +
                                    "/l rmv n=berry data - remove coin by name\n" +
                                    "/pa add id=berry-data&c=usd&t=drops(rises)&p=10 - listen for price drops/rises \n" +
                                    "/pa rmv id=berry-data - remove listen";
            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove()
            );
        }
    }
}
