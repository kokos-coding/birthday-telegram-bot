using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using Birthday.Telegram.Bot.ApplicationServices.Commands;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Processor for bot chat members
/// </summary>
public class BotChatMemberProcessor : IBotProcessor<ChatMemberUpdated>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    public BotChatMemberProcessor(ITelegramBotClient telegramBotClient, IMediator mediator)
    {
        _telegramBotClient = telegramBotClient;
        _mediator = mediator;
    }

    /// <inheritdoc cref="ProcessAsync"/>
    public async Task ProcessAsync(ChatMemberUpdated action, CancellationToken cancellationToken)
    {
        // Спросить текущую инфу о боте
        var botInfo = await _telegramBotClient.GetMeAsync(cancellationToken);
        var processStatus = action.NewChatMember.Status;
        if (botInfo.Id.Equals(action.NewChatMember.User.Id))
        {
            var processUpdate = processStatus switch
            {
                ChatMemberStatus.Member => AddBotToChatAction(chatInfo: action.Chat,
                    botInfo: botInfo,
                    cancellationToken: cancellationToken),
                ChatMemberStatus.Left => RemoveBotFromChatAction(chatInfo: action.Chat,
                    cancellationToken: cancellationToken),

                ChatMemberStatus.Creator => throw new NotImplementedException(),
                ChatMemberStatus.Administrator => throw new NotImplementedException(),
                ChatMemberStatus.Kicked => RemoveBotFromChatAction(chatInfo: action.Chat,
                    cancellationToken: cancellationToken),
                ChatMemberStatus.Restricted => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            await processUpdate;
        }
        else
        {
            var processUpdate = processStatus switch
            {
                ChatMemberStatus.Member => AddNewMemberChatAction(chatInfo: action.Chat,
                    botInfo: botInfo,
                    newUser: action.NewChatMember.User,
                    cancellationToken: cancellationToken),
                ChatMemberStatus.Left => RemoveMemberFromChatAction(chatInfo: action.Chat,
                    cancellationToken: cancellationToken),

                ChatMemberStatus.Creator => throw new NotImplementedException(),
                ChatMemberStatus.Administrator => throw new NotImplementedException(),
                ChatMemberStatus.Kicked => RemoveMemberFromChatAction(chatInfo: action.Chat,
                    cancellationToken: cancellationToken),
                ChatMemberStatus.Restricted => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            await processUpdate;
        }
    }

    private async Task AddBotToChatAction(Chat chatInfo, User botInfo, CancellationToken cancellationToken)
    {
        if (chatInfo.Username != Constants.ChatNameForBirthday)
        {
            // Записать в БД данные о чате
            await _mediator.Send(new CreateChatCommand()
            {
                ChatId = chatInfo.Id,
                DiscussionChatId = null
            }, cancellationToken);
            // Отправить приветственное сообщение
            await SendHelloMessageFromBotToChat(chatInfo, botInfo, cancellationToken);
        }
        return;
    }

    /// <summary>
    /// Действия при удалении бота из чата
    /// </summary>
    /// <param name="chatInfo">Информация о чате из которого удалили бота</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача</returns>
    private async Task RemoveBotFromChatAction(Chat chatInfo, CancellationToken cancellationToken)
    {
        if (chatInfo.Username != Constants.ChatNameForBirthday)
        {
            // Записать в БД данные о чате
            await _mediator.Send(new CreateChatCommand()
            {
                ChatId = chatInfo.Id,
                DiscussionChatId = null
            }, cancellationToken);
        }
        // При удалении нужно вычистить все из бд
        return;
    }

    /// <summary>
    /// Действие при добавлении нового пользователя в чат
    /// </summary>
    /// <param name="chatInfo">Информация о чате в который добавлен пользователь</param>
    /// <param name="newUser">Информация о новом пользователе</param>
    /// <param name="botInfo">Информация о новом боте</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача</returns>
    private async Task AddNewMemberChatAction(Chat chatInfo, User botInfo, User newUser, CancellationToken cancellationToken)
    {
        // Приветствуем нового пользователя чата
        await SendHelloMessageToNewChatMember(chatInfo: chatInfo,
            newUser: newUser,
            botInfo: botInfo,
            cancellationToken: cancellationToken);
        // Записываем данные о пользователе в БД и связываем его с чатом
    }

    /// <summary>
    /// Действие при удалении пользователя из чата
    /// </summary>
    /// <param name="chatInfo">Информация о чате из которого удален пользователь</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача</returns>
    private Task RemoveMemberFromChatAction(Chat chatInfo, CancellationToken cancellationToken)
    {
        // Удаляем пользователя из БД и удаляем связку с ним
        return Task.CompletedTask;
    }

    private Task SendHelloMessageFromBotToChat(Chat chatInfo, User botInfo, CancellationToken cancellationToken) =>
        _telegramBotClient.SendTextMessageAsync(chatInfo.Id,
                        Messages.HelloMessageFromBotToMainChat(botInfo.Username!, chatInfo.Id.ToString()),
                        Messages.ParseMode,
                        cancellationToken: cancellationToken);

    private Task SendHelloMessageToNewChatMember(Chat chatInfo, User botInfo, User newUser, CancellationToken cancellationToken) =>
        _telegramBotClient.SendTextMessageAsync(chatInfo.Id,
                        Messages.HelloMessageForNewChatUser(newUser.Username!, botInfo.Username!, chatInfo.Id.ToString()),
                        Messages.ParseMode,
                        cancellationToken: cancellationToken);
}
