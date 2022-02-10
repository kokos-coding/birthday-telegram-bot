using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers.UnlinkMembersFromChat;

/// <summary>
/// Обработчик события по удалению пользователей из чата
/// </summary>
public class UnlinkMembersFromChatCommandHandler : BaseHandler<UnlinkMembersFromChatCommand, IUnitOfWork>
{   
    private IUnitOfWork UnitOfWork => Service;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="unitOfWork">Экземпляр единицы работы</param>
    /// <param name="logger">Логгер</param>
    public UnlinkMembersFromChatCommandHandler(IUnitOfWork unitOfWork, 
        ILogger<UnlinkMembersFromChatCommandHandler> logger) : base(unitOfWork, logger)
    {
        
    }

    /// <inheritdoc />
    public override Task<Unit> Handle(UnlinkMembersFromChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
