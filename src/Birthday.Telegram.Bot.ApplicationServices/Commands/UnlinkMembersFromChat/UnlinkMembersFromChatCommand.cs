using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Команда для удаления пользователей из чата
/// </summary>
public class UnlinkMemberFromChatCommand : IRequest
{
    /// <summary>
    /// Идентификаторы пользователей для удаления из чата
    /// </summary>
    /// <value></value>
    public IReadOnlyCollection<long> MembersIds { get; init; }
}
