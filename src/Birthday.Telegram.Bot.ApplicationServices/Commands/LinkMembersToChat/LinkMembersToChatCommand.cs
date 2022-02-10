using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Добавление пользователей в чат
/// </summary>
public class LinkMembersToChatCommand : IRequest
{
    /// <summary>
    /// Идентификатор чата в мессенджере
    /// </summary>
    public long ChatId { get; init; }

    /// <summary>
    /// Идентификаторы пользователей для добавления
    /// </summary>
    /// <value></value>
    public IReadOnlyCollection<long> MembersIds { get; init; } = new List<long>();
}
