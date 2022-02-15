using Birthday.Telegram.Bot.ApplicationServices.Queries;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers;

/// <summary>
/// Обработчик события на получение данных о пользователе чата
/// </summary>
public class GetMemberByMemberIdQueryHandler : BaseHandler<GetMemberByMemberIdQuery, GetMemberQueryResponse, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    /// <summary>
    /// Конструтор
    /// </summary>
    /// <param name="unitOfWork">Экземпляр объекта IUnitOfWork</param>
    /// <param name="logger">Экземпляр объекта ILogger</param>
    public GetMemberByMemberIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetMemberByMemberIdQueryHandler> logger) : base(unitOfWork, logger)
    {

    }

    /// <inheritdoc />
    public override async Task<GetMemberQueryResponse> Handle(GetMemberByMemberIdQuery request, CancellationToken cancellationToken)
    {
        var userInDb = await UnitOfWork.ChatMemberRepository.GetByChatMemberIdAsync(request.MemberId, cancellationToken);

        if (userInDb is null)
            throw new EntityNotFoundException($"Member with id {request.MemberId} not found in store");
        return new GetMemberQueryResponse()
        {
            Id = userInDb.Id,
            MemberId = userInDb.MemberId,
            Username = userInDb.Username,
            BirthDay = userInDb.BirthDay
        };
    }
}
