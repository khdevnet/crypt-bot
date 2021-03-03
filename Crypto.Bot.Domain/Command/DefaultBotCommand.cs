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
                                    "/inline   - send inline keyboard\n" +
                                    "/keyboard - send custom keyboard\n" +
                                    "/photo    - send a photo\n" +
                                    "/request  - request location or contact";
            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove()
            );
        }
    }
}
