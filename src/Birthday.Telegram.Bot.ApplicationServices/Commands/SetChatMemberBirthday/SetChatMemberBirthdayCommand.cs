using MediatR;

namespace Birthday.Telegram.Bot.ApplicationServices.Commands;

/// <summary>
/// Команда на обновление даты рождения у пользователя чата
/// </summary>
public class SetChatMemberBirthdayCommand : IRequest
{
    /// <summary>
    /// Идентификатор пользователя в чате
    /// </summary>
    public long ChatMemberId { get; set; }
    
    /// <summary>
    /// День рождения пользователя
    /// </summary>
    public DateTime Birthday { get; set; }
}
