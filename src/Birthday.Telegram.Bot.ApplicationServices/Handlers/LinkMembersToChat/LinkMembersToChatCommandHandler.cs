using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers.LinkMembersToChat;

/// <summary>
/// Обработчик события по добавлению пользователей в чат
/// </summary>
public class LinkMembersToChatCommandHandler : BaseHandler<LinkMembersToChatCommand, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="unitOfWork">Экземпляр единицы работы</param>
    /// <param name="logger">Логгер</param>
    public LinkMembersToChatCommandHandler(IUnitOfWork unitOfWork,
        ILogger<LinkMembersToChatCommandHandler> logger) : base(unitOfWork, logger)
    { }

    /// <inheritdoc />
    public override async Task<Unit> Handle(LinkMembersToChatCommand request, CancellationToken cancellationToken)
    {
        var chatInfo = await UnitOfWork.ChatRepository.GetByChatIdAsync(request.ChatId, cancellationToken);
        if(chatInfo is null)
            throw new EntityNotFoundException($"Chat with id {request.ChatId} not found in store");
        var memberInfo = await UnitOfWork
            .ChatMemberRepository
            .GetByChatMembersIdsAsync(request.MembersIds.ToList(), cancellationToken);
        
        await UnitOfWork.StartTransactionAsync(cancellationToken);
        await UnitOfWork.ChatRepository.LinkChatMembersToChatAsync(chatInfo, memberInfo, cancellationToken);
        await UnitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
