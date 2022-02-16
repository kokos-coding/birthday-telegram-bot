using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.AggregationModels;
using Birthday.Telegram.Bot.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Birthday.Telegram.Bot.DataAccess;

/// <inheritdoc cref="IUnitOfWork" />
public class UnitOfWork : IUnitOfWork
{
    /// <inheritdoc cref="ChatRepository" />
    public IChatRepository ChatRepository { get; }

    /// <inheritdoc cref="ChatMemberRepository" />
    public IChatMemberRepository ChatMemberRepository { get; }

    private readonly BirthdayBotDbContext _dbContext;
    private IDbContextTransaction? _dbContextTransaction = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">DbContext instance</param>
    /// <param name="chatRepository">Instance of ChatRepository</param>
    /// <param name="chatMemberRepository">Instance of ChatMemberRepository</param>
    public UnitOfWork(BirthdayBotDbContext dbContext,
        IChatRepository chatRepository, 
        IChatMemberRepository chatMemberRepository)
    {
        _dbContext = dbContext;
        ChatRepository = chatRepository;
        ChatMemberRepository = chatMemberRepository;
    }

    /// <inheritdoc cref="StartTransactionAsync"/>
    public async ValueTask StartTransactionAsync(CancellationToken cancellationToken)
    {
        _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc cref="CommitAsync"/>
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if(_dbContextTransaction is null)
            throw new Exception("Transaction is not started. Please before commit changes start transaction");
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }
}
