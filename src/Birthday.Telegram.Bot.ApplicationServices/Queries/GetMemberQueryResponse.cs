using Birthday.Telegram.Bot.Domain.Models.Abstractions;

namespace Birthday.Telegram.Bot.ApplicationServices.Queries;

/// <summary>
/// Результат получения сведений о пользователе
/// </summary>
public class GetMemberQueryResponse : IIdModel<long>
{
    /// <inheritdoc cref="Id"/>
    public long Id { get; set; }

    /// <summary>
    /// Telegram user id
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// Chat member name
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Birth Day of member
    /// </summary>
    public DateTime? BirthDay { get; set; }
}
