using Birthday.Telegram.Bot.Domain.Models.Abstractions;

namespace Birthday.Telegram.Bot.Domain.AggregationModels;

/// <summary>
/// Model that represents member of chat
/// </summary>
public class ChatMember : IIdModel<long>
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

    /// <summary>
    /// Link to chats for user
    /// </summary>
    public ICollection<ChatChatMember> GroupChatChatMembers { get; set; } = new List<ChatChatMember>();
}
