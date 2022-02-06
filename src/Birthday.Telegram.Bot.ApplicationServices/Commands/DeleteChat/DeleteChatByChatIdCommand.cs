using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Удалить чат по его идентификатору
/// </summary>
public class DeleteChatByChatIdCommand : IRequest
{
    /// <summary>
    /// Идентификатор чата
    /// </summary>
    public long ChatId { get; set; }
}
