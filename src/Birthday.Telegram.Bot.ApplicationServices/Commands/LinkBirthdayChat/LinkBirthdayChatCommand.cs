using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands.UpdateChat;

/// <summary>
/// Команда на добавление к чату сведений о чате для поздравлений
/// </summary>
public class LinkBirthdayChatCommand : IRequest
{
    /// <summary>
    /// Идентификатор чата к которому добавляется чат для поздравлений
    /// </summary>
    public long ChatId { get; init; }

    /// <summary>
    /// Идентификатор чата для поздравлений
    /// </summary>
    public long BirthdayChatId { get; init; }
}
