using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Добавление пользователей в чат
/// </summary>
public class LinkMembersToChatCommand : IRequest
{
    /// <summary>
    /// Идентификаторы пользователей для добавления
    /// </summary>
    /// <value></value>
    public IReadOnlyCollection<long> MembersIds { get; init; }
}
