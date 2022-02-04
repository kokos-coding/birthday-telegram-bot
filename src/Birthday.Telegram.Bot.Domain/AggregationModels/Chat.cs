using Birthday.Telegram.Bot.Domain.Models.Abstractions;

namespace Birthday.Telegram.Bot.Domain.AggregationModels;

/// <summary>
/// Model that represent chat information
/// </summary>
public class Chat : IIdModel<long>
{
    /// <inheritdoc cref="Id"/>
    public long Id { get; set; }

    /// <summary>
    /// Identifier of main chat id
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Discussion chat id
    /// </summary>
    public long DiscussionChatId { get; set; }

    /// <summary>
    /// Link to main chat members
    /// </summary>
    public ICollection<ChatChatMember> GroupChatChatMembers { get; set; } = new List<ChatChatMember>();
}
