using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Services.Abstractions;

/// <summary>
/// Service for processing messages coming from the bot
/// </summary>
public interface IBotUpdateService
{
    /// <summary>
    /// Method for processing message from bot
    /// </summary>
    /// <param name="update">Message update</param>
    /// <param name="cancellationToken">Cancellation token instance</param>
    /// <returns>Completed task</returns>
    Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken);
}
