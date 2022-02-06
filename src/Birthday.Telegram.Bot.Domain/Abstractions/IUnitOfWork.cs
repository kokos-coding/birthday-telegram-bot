using Birthday.Telegram.Bot.Domain.AggregationModels;

namespace Birthday.Telegram.Bot.Domain.Abstractions;

/// <summary>
/// Interface for IoW 
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Chat repository property
    /// </summary>
    IChatRepository ChatRepository { get; }

    /// <summary>
    /// Chat member repository
    /// </summary>
    IChatMemberRepository ChatMemberRepository { get; }

    /// <summary>
    /// Start transaction on db connection
    /// </summary>
    ValueTask StartTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Commit all changes
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken);
}
