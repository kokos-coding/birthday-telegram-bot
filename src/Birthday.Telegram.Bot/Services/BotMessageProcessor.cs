using System.Globalization;
using Birthday.Telegram.Bot.Controls;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using Birthday.Telegram.Bot.ApplicationServices.Queries;
using Birthday.Telegram.Bot.ApplicationServices.Commands;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Birthday.Telegram.Bot.Domain.Exceptions;
using Birthday.Telegram.Bot.Domain.Abstractions;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Processor for bot massages
/// </summary>
public class BotMessageProcessor : IBotProcessor<Message>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IMediator _mediator;
    private readonly ILogger<BotMessageProcessor> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="telegramBotClient">Instance of telegram bot client</param>
    /// <param name="logger">Instance of Logger</param>
    /// <param name="mediator">Instance of mediator</param>
    public BotMessageProcessor(ITelegramBotClient telegramBotClient,
        ILogger<BotMessageProcessor> logger,
        IMediator mediator)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task ProcessAsync(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {messageType}", message.Type);
        if (message.Type != MessageType.Text)
            return Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(message.Text))
            return SendErrorMessageAsync(chatId: message.Chat.Id,
                            errorMessage: Messages.ErrorMessages.EmptyMessage,
                            cancellationToken: cancellationToken);

        var splittedReceivedText = message.Text!.Split(' ').ToList();
        var receivedCommand = splittedReceivedText.First();

        var action = receivedCommand switch
        {
            Constants.BotCommands.Start => SendHelloMessageAsync(message.Chat,
                                                    splittedReceivedText.Skip(1).ToArray(),
                                                    cancellationToken),
            _ => SendErrorMessageAsync(chatId: message.Chat.Id,
                    errorMessage: Messages.ErrorMessages.MessageCouldNotRecognized,
                    cancellationToken: cancellationToken)
        };

        return action;
    }

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
            try
            {
                var userInChat = await _telegramBotClient.GetChatMemberAsync(chatId: mainChatId,
                                userId: chatInfo.Id,
                                cancellationToken: cancellationToken);
                var userInDb = await _mediator.Send(new GetMemberByMemberIdQuery()
                {
                    MemberId = userInChat.User.Id
                }, cancellationToken);

                if(userInDb.BirthDay is not null)
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: @$"И снова здравствуйте\, {chatInfo.Username}\!
У меня уже есть все данные о Вас\, поэтому уже не нужно снова вводить свою дату рождения\.",
                                                parseMode: Messages.ParseMode,
                                                cancellationToken: cancellationToken);
                return;
                }
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation("User with id {memberId} not found in store, start create it. Error message {exception}", chatInfo.Id, ex);
                await _mediator.Send(new CreateChatMemberCommand()
                {
                    ChatMemberId = chatInfo.Id,
                    Username = chatInfo.Username,
                    Birthday = null,
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("User with id {userId} from chat {chatId} not in this chat. Exception: {exception}", 
                    chatInfo.Id, 
                    mainChatId, 
                    ex.Message);
                await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                            text: Messages.ErrorMessages.UserNotInChat,
                            Messages.ParseMode,
                            cancellationToken: cancellationToken);
                return;
            }

            // Отправляем приветственное сообщение
            await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: Messages.MessageForGetBirthdayDate(chatInfo.Username!, mainChatInfo.Title!),
                                                parseMode: Messages.ParseMode,
                                                cancellationToken: cancellationToken);

            var keyboard = CalendarPicker.InitializeCalendarPickerKeyboard(DateTime.Now, CultureInfo.CurrentCulture);

            await _telegramBotClient.SendTextMessageAsync(chatId: chatInfo.Id,
                                                text: "Выберете дату своего рождения",
                                                replyMarkup: keyboard,
                                                cancellationToken: cancellationToken);
            return;
        }

        await SendErrorMessageAsync(chatId: chatInfo.Id,
                    errorMessage: Messages.ErrorMessages.MessageCouldNotRecognized,
                    cancellationToken: cancellationToken);
    }

    private Task SendErrorMessageAsync(ChatId chatId, string errorMessage, CancellationToken cancellationToken) =>
        _telegramBotClient.SendTextMessageAsync(chatId: chatId,
                                        text: @$"Упс\, произошла непонятка\.
{errorMessage}",
                                        parseMode: Messages.ParseMode,
                                        cancellationToken: cancellationToken);
}