using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Queries;

/// <summary>
/// Запрос на получение сведений о чате по идентификатору чата в мессенджере
/// </summary>
public class GetChatByChatIdQuery : IRequest<GetChatQueryResponse>
{
    /// <summary>
    /// Идентификатор чата в мессенджере
    /// </summary>
    /// <value></value>
    public long ChatId { get; init; }
}
