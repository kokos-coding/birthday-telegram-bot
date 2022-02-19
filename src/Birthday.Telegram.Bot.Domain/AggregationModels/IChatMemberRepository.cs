namespace Birthday.Telegram.Bot.Domain.AggregationModels;

/// <summary>
/// Repository for manage chat members
/// </summary>
public interface IChatMemberRepository
{
    /// <summary>
    /// Create new Chat member in store
    /// </summary>
    /// <param name="value">Model for create new chat member</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    /// <returns>Identifier of new chat row</returns>
    Task<long> CreateAsync(ChatMember value, CancellationToken cancellationToken);

    /// <summary>
    /// Update chat member information
    /// </summary>
    /// <param name="value">Chat member model for update</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task UpdateAsync(ChatMember value, CancellationToken cancellationToken);

    /// <summary>
    /// Delete chat member row by chat info
    /// </summary>
    /// <param name="value">Chat member row for delete</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task DeleteAsync(ChatMember value, CancellationToken cancellationToken);

    /// <summary>
    /// Delete chat member row by chat id
    /// </summary>
    /// <param name="chatMemberId">Chat member id</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    Task DeleteAsync(long chatMemberId, CancellationToken cancellationToken);

    /// <summary>
    /// Get member info by chat member id
    /// </summary>
    /// <param name="chatMemberId">Chat member id for search</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    /// <returns>Information about chat member</returns>
    Task<ChatMember?> GetByChatMemberIdAsync(long chatMemberId, CancellationToken cancellationToken);

    /// <summary>
    /// Get members info by chat member id
    /// </summary>
    /// <param name="chatMembersIds">Chat members ids for search</param>
    /// <param name="cancellationToken">Instance of Cancellation token</param>
    /// <returns>Information about chat members</returns>
    Task<List<ChatMember>> GetByChatMembersIdsAsync(ICollection<long> chatMembersIds, CancellationToken cancellationToken);
}
