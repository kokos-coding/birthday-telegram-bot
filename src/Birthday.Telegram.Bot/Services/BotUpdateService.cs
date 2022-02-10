using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Controls;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace Birthday.Telegram.Bot.Services;

/// <inheritdoc cref="IBotUpdateService" />
public class BotUpdateService : IBotUpdateService
{
    private readonly ILogger<BotUpdateService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IMediator _mediator;

    private readonly IBotProcessor<ChatMemberUpdated> _botChatMemberProcessor;
    private readonly IBotProcessor<Message> _botMessageProcessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="botClient">Bot client instance</param>
    /// <param name="mediator">Mediator</param>
    /// <param name="logger">Logger</param>
    /// <param name="botChatMemberProcessor">Instance of processor for chat members</param>
    /// <param name="botMessageProcessor">Instance of processor for messages</param>
    public BotUpdateService(
        ITelegramBotClient botClient,
        IMediator mediator,
        ILogger<BotUpdateService> logger,
        IBotProcessor<ChatMemberUpdated> botChatMemberProcessor,
        IBotProcessor<Message> botMessageProcessor)
    {
        _botClient = botClient;
        _mediator = mediator;
        _logger = logger;
        _botChatMemberProcessor = botChatMemberProcessor;
        _botMessageProcessor = botMessageProcessor;
    }

    /// <inheritdoc cref="ProcessUpdateAsync" />
    public async Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var updateTemp = update;

        var handler = update.Type switch
        {
            UpdateType.MyChatMember => _botChatMemberProcessor.ProcessAsync(update.MyChatMember!, cancellationToken),
            UpdateType.Message => _botMessageProcessor.ProcessAsync(update.Message!, cancellationToken),
            UpdateType.EditedMessage => _botMessageProcessor.ProcessAsync(update.EditedMessage!, cancellationToken),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!, cancellationToken),
            UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery!),
            UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),

            //UpdateType
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (CalendarPicker.IsCalendarPickerCommand(callbackQuery.Data!))
        {
            var processorResult = CalendarPicker.CalendarPickerProcessor(callbackQuery.Data!);
            if (processorResult is null)
            {
                await _botClient.DeleteMessageAsync(chatId: callbackQuery.Message!.Chat.Id,
                    messageId: callbackQuery.Message.MessageId,
                    cancellationToken: cancellationToken);
                await SendServerErrorMessage(callbackQuery.Message!.Chat.Id, cancellationToken);
                return;
            }
            switch (processorResult.ResultType)
            {
                case CalendarPicker.ProcessorResultType.Date:
                    var result = await _mediator.Send(new CreateChatMemberCommand()
                    {
                        ChatMemberId = callbackQuery.From.Id,
                        Username = callbackQuery.From.Username,
                        Birthday = processorResult.TargetDate
                    }, cancellationToken);
                    
                    return;
                case CalendarPicker.ProcessorResultType.KeyboardMarkup:
                    await _botClient.EditMessageReplyMarkupAsync(callbackQuery.Message!.Chat.Id,
                            messageId: callbackQuery.Message.MessageId,
                            replyMarkup: processorResult.KeyboardMarkup,
                            cancellationToken: cancellationToken);
                    return;
            }
        }

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}");

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}");
    }

    #region Inline Mode

    private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
    {
        _logger.LogInformation("Received inline query from: {inlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

        await _botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                results: results,
                                                isPersonal: true,
                                                cacheTime: 0);
    }

    private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
    {
        _logger.LogInformation("Received inline result: {chosenInlineResultId}", chosenInlineResult.ResultId);
        return Task.CompletedTask;
    }

    #endregion

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {updateType}", update.Type);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handle raised error in system
    /// </summary>
    /// <param name="exception">Current exception</param>
    public Task HandleErrorAsync(Exception exception)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }

    private Task SendServerErrorMessage(ChatId chatId, CancellationToken cancellationToken) => 
            _botClient.SendTextMessageAsync(chatId: chatId,
                                        text: @$"Упс\, произошла непонятка\.
{Messages.ErrorMessages.ServerError}",
                                        parseMode: Messages.ParseMode,
                                        cancellationToken: cancellationToken);
}