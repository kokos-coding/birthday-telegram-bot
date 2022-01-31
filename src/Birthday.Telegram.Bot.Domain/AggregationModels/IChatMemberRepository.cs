using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
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
        Task DeleteAsync(int chatMemberId, CancellationToken cancellationToken);

        /// <summary>
        /// Get chat info by chat member id
        /// </summary>
        /// <param name="chatMemberId">Chat member i for search</param>
        /// <param name="cancellationToken">Instance of Cancellation token</param>
        /// <returns>Information about chat member</returns>
        Task<ChatMember?> GetByChatMemberId(int chatMemberId, CancellationToken cancellationToken);
    }
}