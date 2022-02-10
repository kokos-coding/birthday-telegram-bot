using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Abstractions;
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
    {

    }

    /// <inheritdoc />
    public override Task<Unit> Handle(LinkMembersToChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
