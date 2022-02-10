using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Queries;

/// <summary>
/// Получение данных о пользователе по его идентификатору в мессенджере
/// </summary>
public class GetMemberByMemberIdQuery : IRequest<GetMemberQueryResponse>
{
    /// <summary>
    /// Идентификатор пользователя мессенджера
    /// </summary>
    public long MemberId { get; init; }
}
