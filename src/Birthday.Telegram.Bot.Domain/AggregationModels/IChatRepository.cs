using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
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
        public Task<long> CreateAsync(Chat value, CancellationToken cancellationToken);

        /// <summary>
        /// Update chat information
        /// </summary>
        /// <param name="value">Chat model for update</param>
        /// <param name="cancellationToken">Instance of Cancellation token</param>
        public Task UpdateAsync(Chat value, CancellationToken cancellationToken);

        /// <summary>
        /// Delete chat row by chat info
        /// </summary>
        /// <param name="value">Chat row for delete</param>
        /// <param name="cancellationToken">Instance of Cancellation token</param>
        public Task DeleteAsync(Chat value, CancellationToken cancellationToken);

        /// <summary>
        /// Delete chat row by chat id
        /// </summary>
        /// <param name="chatId">Chat id</param>
        /// <param name="cancellationToken">Instance of Cancellation token</param>
        public Task DeleteAsync(int chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Get chat info by chat id
        /// </summary>
        /// <param name="chatId">Chat id for search</param>
        /// <param name="cancellationToken">Instance of Cancellation token</param>
        /// <returns>Information about chat</returns>
        public Task<Chat> GetByChatId(int chatId, CancellationToken cancellationToken);
    }
}