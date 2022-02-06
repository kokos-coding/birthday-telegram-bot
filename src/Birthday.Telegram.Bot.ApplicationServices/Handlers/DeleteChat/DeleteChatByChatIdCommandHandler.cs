using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers;

/// <summary>
/// Обработчик команды на удаление чата
/// </summary>
public class DeleteChatByChatIdCommandHandler : BaseHandler<DeleteChatByChatIdCommand, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    public DeleteChatByChatIdCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteChatByChatIdCommandHandler> logger) : base(unitOfWork, logger)
    {
    }

    public async override Task<Unit> Handle(DeleteChatByChatIdCommand request, CancellationToken cancellationToken)
    {
        await UnitOfWork.StartTransactionAsync(cancellationToken);
        var chatInDb = await UnitOfWork.ChatRepository.GetByChatId(request.ChatId, cancellationToken);
        if(chatInDb is null)
            throw new Exception($"Chat with id {request.ChatId} not found in store");
        await UnitOfWork.ChatRepository.DeleteAsync(chatInDb, cancellationToken);
        await UnitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}