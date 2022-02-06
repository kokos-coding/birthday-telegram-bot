using Birthday.Telegram.Bot.Domain.Models;
using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

public class CreateChatCommand : IRequest<IdModel<long>>
{
    /// <summary>
    /// Identifier of main chat id
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Discussion chat id
    /// </summary>
    public long DiscussionChatId { get; set; }
}
