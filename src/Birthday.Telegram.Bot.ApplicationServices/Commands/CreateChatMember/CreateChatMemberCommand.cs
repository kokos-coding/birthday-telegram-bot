using Birthday.Telegram.Bot.Domain.Models;
using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Команда для создания записи участника чата
/// </summary>
public class CreateChatMemberCommand : IRequest<IdModel<long>>
{
    /// <summary>
    /// Идентификатор участника чата
    /// </summary>
    public long ChatMemberId { get; set; }

    /// <summary>
    /// Имя участника чата
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? Birthday { get; set; }
}
