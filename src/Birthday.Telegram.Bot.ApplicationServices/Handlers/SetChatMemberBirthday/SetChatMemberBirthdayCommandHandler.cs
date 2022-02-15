using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers;

/// <summary>
/// Обработчик команды на обновление даты рождения пользователя
/// </summary>
public class SetChatMemberBirthdayCommandHandler : BaseHandler<SetChatMemberBirthdayCommand, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    /// <summary>
    /// Конструтор
    /// </summary>
    /// <param name="unitOfWork">Экземпляр объекта IUnitOfWork</param>
    /// <param name="logger">Экземпляр объекта ILogger</param>
    public SetChatMemberBirthdayCommandHandler(IUnitOfWork unitOfWork, ILogger<SetChatMemberBirthdayCommandHandler> logger) : base(unitOfWork, logger)
    {

    }

    /// <inheritdoc/>
    public override async Task<Unit> Handle(SetChatMemberBirthdayCommand request, CancellationToken cancellationToken)
    {
        var userInDb = await UnitOfWork.ChatMemberRepository.GetByChatMemberIdAsync(request.ChatMemberId, cancellationToken);
        if(userInDb is null)
            throw new EntityNotFoundException($"User with member id {request.ChatMemberId} not found in store");

        await UnitOfWork.StartTransactionAsync(cancellationToken);
        userInDb.BirthDay = request.Birthday;

        await UnitOfWork.ChatMemberRepository.UpdateAsync(userInDb, cancellationToken);
        await UnitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
