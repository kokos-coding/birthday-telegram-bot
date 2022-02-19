using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Controls;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Обработчик на Callback действия пришедшие с клавиатуры
/// </summary>
public class BotCallbackQueryProcessor : IBotProcessor<CallbackQuery>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMediator _mediator;
    private readonly ILogger<BotCallbackQueryProcessor> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="botClient">Экземпляр клиента телеграм бота</param>
    /// <param name="mediator">Экземпляр медиатора</param>
    /// <param name="logger">Экземпляр логгера</param>
    public BotCallbackQueryProcessor(ITelegramBotClient botClient,
            IMediator mediator,
            ILogger<BotCallbackQueryProcessor> logger)
    {
        _botClient = botClient;
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc cref="ProcessAsync" />
    public async Task ProcessAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
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
                    // Добавляем дату рождения пользователю
                    var result = await _mediator.Send(new SetChatMemberBirthdayCommand()
                    {
                        ChatMemberId = callbackQuery.From.Id,
                        Birthday = processorResult.TargetDate.Value
                    }, cancellationToken);
                    // Удаляем клавиатуру
                    await _botClient.DeleteMessageAsync(chatId: callbackQuery.Message!.Chat.Id,
                        messageId: callbackQuery.Message.MessageId,
                        cancellationToken: cancellationToken);
                    // Пишем сообщение о том, что дата рождения сохранена. И что можно дальше работать
                    await _botClient.SendTextMessageAsync(chatId: callbackQuery.Message!.Chat.Id,
                        text: Messages.MessageAfterSaveBirthdayDate(),
                        parseMode: Messages.ParseMode,
                        cancellationToken: cancellationToken);

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

    private Task SendServerErrorMessage(ChatId chatId, CancellationToken cancellationToken) =>
            _botClient.SendTextMessageAsync(chatId: chatId,
                                        text: @$"Упс\, произошла непонятка\.
{Messages.ErrorMessages.ServerError}",
                                        parseMode: Messages.ParseMode,
                                        cancellationToken: cancellationToken);
}
