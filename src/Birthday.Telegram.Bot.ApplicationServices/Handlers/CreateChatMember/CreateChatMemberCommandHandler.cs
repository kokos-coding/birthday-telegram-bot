using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.AggregationModels;
using Birthday.Telegram.Bot.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers.CreateMember;

/// <summary>
/// Обработчик команды на событие создание чата
/// </summary>
public class CreateChatMemberCommandHandler : BaseHandler<CreateChatMemberCommand, IdModel<long>, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="unitOfWork">Экземпляр единицы работы</param>
    /// <param name="logger">Логгер</param>
    public CreateChatMemberCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateChatMemberCommandHandler> logger) : base(unitOfWork, logger)
    {
    }

    /// <inheritdoc />
    public override async Task<IdModel<long>> Handle(CreateChatMemberCommand request, CancellationToken cancellationToken)
    {
        var newChatMember = new ChatMember()
        {
            Id = 0,
            MemberId = request.ChatMemberId,
            Username = request.Username,
            BirthDay = request.Birthday
        };
        await UnitOfWork.StartTransactionAsync(cancellationToken);
        var result = await UnitOfWork.ChatMemberRepository.CreateAsync(newChatMember, cancellationToken);

        await UnitOfWork.CommitAsync(cancellationToken);
        return new IdModel<long>() { Id = result };
    }
}
