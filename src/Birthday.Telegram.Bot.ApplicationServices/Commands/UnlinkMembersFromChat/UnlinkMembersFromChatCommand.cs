using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Команда для удаления пользователей из чата
/// </summary>
public class UnlinkMembersFromChatCommand : IRequest
{
    /// <summary>
    /// Идентификатор чата в мессенджере
    /// </summary>
    public long ChatId { get; init; }

    /// <summary>
    /// Идентификаторы пользователей для удаления из чата
    /// </summary>
    /// <value></value>
    public IReadOnlyCollection<long> MembersIds { get; init; } = new List<long>();
}
