using Crypto.Bot.Domain.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Crypto.Bot.Api.HostedServices
{
    public class BotHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<BotHostedService> _logger;
        private readonly IConfiguration configuration;
        private readonly TelegramBotClient bot;
        private readonly IEnumerable<IBotCommand> botCommands;

        public BotHostedService(ILogger<BotHostedService> logger, IConfiguration configuration, TelegramBotClient bot, IEnumerable<IBotCommand> botCommands)
        {
            _logger = logger;
            this.configuration = configuration;
            this.bot = bot;
            this.botCommands = botCommands;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");


            var me = await bot.GetMeAsync();
            Console.Title = me.Username;

            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            bot.OnInlineQuery += BotOnInlineQueryReceived;
            bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            bot.OnReceiveError += BotOnReceiveError;

            bot.StartReceiving(Array.Empty<UpdateType>());
            _logger.LogInformation($"Start listening for @{me.Username}");


        }

        private void DoWork(object state)
        {
            _logger.LogInformation(
                "Timed Hosted Service is working.");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            bot.StopReceiving();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            bot?.StopReceiving();
        }

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            var commandName = message.Text.Split(' ').First();

            var command = botCommands.FirstOrDefault(c => c.Name == commandName);
            if (command != null)
            {
                command.Execute(message).GetAwaiter().GetResult();
                return;
            }

            botCommands.FirstOrDefault(c => c.Name == BotCommands.Default).Execute(message).GetAwaiter().GetResult();
        }

        // Process Inline Keyboard callback data
        private void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            bot.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}"
            ).GetAwaiter().GetResult();

            bot.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}"
            ).GetAwaiter().GetResult();
        }

        // Send inline keyboard
        // You can process responses in BotOnCallbackQueryReceived handler
        async Task SendInlineKeyboard(Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            // Simulate longer running task
            await Task.Delay(500);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    }
                });
            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose",
                replyMarkup: inlineKeyboard
            );
        }

        #region Inline Mode

        private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };
            await bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        #endregion

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }
    }
}
