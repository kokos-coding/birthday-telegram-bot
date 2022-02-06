using Birthday.Telegram.Bot.ApplicationServices.Commands;
using Birthday.Telegram.Bot.Domain.Models;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.AggregationModels;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers;

public class CreateChatCommandHandler : BaseHandler<CreateChatCommand, IdModel<long>, IUnitOfWork>
{
    private IUnitOfWork UnitOfWork => Service;

    public CreateChatCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateChatCommandHandler> logger) : base(unitOfWork, logger) 
    {
    }

    /// <inheritdoc />
    public override async Task<IdModel<long>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var newChat = new Chat()
        {
            Id = 0,
            ChatId = request.ChatId,
            DiscussionChatId = request.DiscussionChatId
        };

        await UnitOfWork.StartTransactionAsync(cancellationToken);
        var result = await UnitOfWork.ChatRepository.CreateAsync(newChat, cancellationToken);
        await UnitOfWork.CommitAsync(cancellationToken);
        return new IdModel<long> { Id = result };
    }
}