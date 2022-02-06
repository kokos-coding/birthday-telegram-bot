using Birthday.Telegram.Bot.Controls;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Processor for bot massages
/// </summary>
public class BotMessageProcessor : IBotMessageProcessor
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BotMessageProcessor> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="telegramBotClient">Instance of telegram bot client</param>
    /// <param name="logger">Instance of Logger</param>
    public BotMessageProcessor(ITelegramBotClient telegramBotClient,
        ILogger<BotMessageProcessor> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task ProcessAsync(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {messageType}", message.Type);
        if (message.Type != MessageType.Text)
            return Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(message.Text))
            return SendErrorMessageAsync(message.Chat.Id,
                            "Полученное сообщение пустое... Я не понимаю что вы хотите мне сказать",
                            cancellationToken);

        var splittedReceivedText = message.Text!.Split(' ').ToList();
        var receivedCommand = splittedReceivedText.First();

        var action = receivedCommand switch
        {
            Constants.BotCommands.Start => SendHelloMessageAsync(message.Chat,
                                                    splittedReceivedText.Skip(1).ToArray(),
                                                    cancellationToken),
            _ => SendErrorMessageAsync(message.Chat.Id, "Я не распознал выше сообщение\\. Введите /start", cancellationToken)
        };

        return Task.CompletedTask;
        //     var action = message.Text!.Split(' ')[0] switch
        //     {
        //         "/inline" => SendInlineKeyboard(_botClient, message),
        //         "/keyboard" => SendReplyKeyboard(_botClient, message),
        //         "/remove" => RemoveKeyboard(_botClient, message),
        //         "/photo" => SendFile(_botClient, message),
        //         "/request" => RequestContactAndLocation(_botClient, message),
        //         _ => Usage(_botClient, message)
        //     };
        //     Message sentMessage = await action;
        //     _logger.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);

        //     // Send inline keyboard
        //     // You can process responses in BotOnCallbackQueryReceived handler
        //     static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
        //     {
        //         await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

        //         // Simulate longer running task
        //         await Task.Delay(500);

        //         InlineKeyboardMarkup inlineKeyboard = new(
        //             new[]
        //             {
        //                 // first row
        //                 new []
        //                 {
        //                     InlineKeyboardButton.WithCallbackData("1.1", "11"),
        //                     InlineKeyboardButton.WithCallbackData("1.2", "12"),
        //                 },
        //                 // second row
        //                 new []
        //                 {
        //                     InlineKeyboardButton.WithCallbackData("2.1", "21"),
        //                     InlineKeyboardButton.WithCallbackData("2.2", "22"),
        //                 },
        //             });

        //         return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
        //                                               text: "Choose",
        //                                               replyMarkup: inlineKeyboard);
        //     }

        //     static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
        //     {
        //         ReplyKeyboardMarkup replyKeyboardMarkup = new(
        //             new[]
        //             {
        //                     new KeyboardButton[] { "1.1", "1.2" },
        //                     new KeyboardButton[] { "2.1", "2.2" },
        //             })
        //         {
        //             ResizeKeyboard = true
        //         };

        //         return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
        //                                               text: "Choose",
        //                                               replyMarkup: replyKeyboardMarkup);
        //     }

        //     static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
        //     {
        //         return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
        //                                               text: "Removing keyboard",
        //                                               replyMarkup: new ReplyKeyboardRemove());
        //     }

        //     static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
        //     {
        //         await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

        //         const string filePath = @"Files/tux.png";
        //         using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //         var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

        //         return await bot.SendPhotoAsync(chatId: message.Chat.Id,
        //                                         photo: new InputOnlineFile(fileStream, fileName),
        //                                         caption: "Nice Picture");
        //     }

        //     static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
        //     {
        //         ReplyKeyboardMarkup RequestReplyKeyboard = new(
        //             new[]
        //             {
        //                 KeyboardButton.WithRequestLocation("Location"),
        //                 KeyboardButton.WithRequestContact("Contact"),
        //             });

        //         return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
        //                                               text: "Who or Where are you?",
        //                                               replyMarkup: RequestReplyKeyboard);
        //     }

        //     static async Task<Message> Usage(ITelegramBotClient bot, Message message)
        //     {
        //         const string usage = "Usage:\n" +
        //                              "/inline   - send inline keyboard\n" +
        //                              "/keyboard - send custom keyboard\n" +
        //                              "/remove   - remove custom keyboard\n" +
        //                              "/photo    - send a photo\n" +
        //                              "/request  - request location or contact";

        //         return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
        //                                               text: usage,
        //                                               replyMarkup: new ReplyKeyboardRemove());
        //     }
        // }
    }

    private Task SendErrorMessageAsync(ChatId chatId, string errorMessage, CancellationToken cancellationToken) =>
        _telegramBotClient.SendTextMessageAsync(chatId: chatId,
                                        text: @$"Упс\, произошла непонятка\.
{errorMessage}",
                                        parseMode: Messages.ParseMode,
                                        cancellationToken: cancellationToken);


    private async Task SendHelloMessageAsync(Chat chatInfo, string[] args, CancellationToken cancellationToken)
    {
        // Проверка, если ID чата больше нуля то тогда это личные сообщения, иначе это групповые чаты
        if (args.Length == 0 && chatInfo.Id > 0)
        {
            _ = _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: Messages.HelloMessage(chatInfo.Username!),
                                                parseMode: Messages.ParseMode,
                                                cancellationToken: cancellationToken);
            return;
        }


        // проверяем что первый аргумент является числом
        if (long.TryParse(args.First(), out var mainChatId))
        {
            var mainChatInfo = await _telegramBotClient.GetChatAsync(chatId: mainChatId,
                                                cancellationToken: cancellationToken);

            await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: Messages.MessageForGetBirthdayDate(chatInfo.Username!, mainChatInfo.Title!),
                                                parseMode: Messages.ParseMode,
                                                cancellationToken: cancellationToken);

            var keyboard = CalendarPicker.InitializeCalendarPickerKeyboard(DateTime.Now);
            try{
            var result = await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: "Выберете дату своего рождения",
                                                replyMarkup: keyboard,
                                                cancellationToken: cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            var asd = 1+ 1;
        }
    }
}