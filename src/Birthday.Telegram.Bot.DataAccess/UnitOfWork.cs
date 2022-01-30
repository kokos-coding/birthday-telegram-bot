using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.AggregationModels;
using Microsoft.EntityFrameworkCore;

namespace Birthday.Telegram.Bot.DataAccess;

/// <inheritdoc cref="IUnitOfWork" />
public class UnitOfWork : IUnitOfWork
{
    /// <inheritdoc cref="ChatRepository" />
    public IChatRepository ChatRepository { get; }

    /// <inheritdoc cref="ChatMemberRepository" />
    public IChatMemberRepository ChatMemberRepository { get; }

    private readonly DbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">DbContext instance</param>
    /// <param name="chatRepository">Instance of ChatRepository</param>
    /// <param name="chatMemberRepository">Instance of ChatMemberRepository</param>
    public UnitOfWork(DbContext dbContext,
        IChatRepository chatRepository, 
        IChatMemberRepository chatMemberRepository)
    {
        _dbContext = dbContext;
        ChatRepository = chatRepository;
        ChatMemberRepository = chatMemberRepository;
    }

    /// <summary>
    /// Commit all changes
    /// </summary>
    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
