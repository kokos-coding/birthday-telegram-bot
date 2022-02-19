namespace Birthday.Telegram.Bot.Domain.AggregationModels;

/// <summary>
/// Repository for manage chats
/// </summary>
public interface IChatRepository
{
    /// <summary>
    /// Create new Chat in store
    /// </summary>
    /// <param name="value">Model for create new chat</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    /// <returns>Identifier of new chat row</returns>
    Task<long> CreateAsync(Chat value, CancellationToken cancellationToken);

    /// <summary>
    /// Update chat information
    /// </summary>
    /// <param name="value">Chat model for update</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task UpdateAsync(Chat value, CancellationToken cancellationToken);

    /// <summary>
    /// Delete chat row by chat info
    /// </summary>
    /// <param name="value">Chat row for delete</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task DeleteAsync(Chat value, CancellationToken cancellationToken);

    /// <summary>
    /// Delete chat row by chat id
    /// </summary>
    /// <param name="chatId">Chat id</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task DeleteAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    /// Get chat info by chat id
    /// </summary>
    /// <param name="chatId">Chat id for search</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    /// <returns>Information about chat</returns>
    Task<Chat?> GetByChatIdAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    /// Link chat members to current chat
    /// </summary>
    /// <param name="chatInfo">Information of target chat</param>
    /// <param name="membersInfos">Members to add collection</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task LinkChatMembersToChatAsync(Chat chatInfo, ICollection<ChatMember> membersInfos, CancellationToken cancellationToken);
}
