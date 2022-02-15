using Birthday.Telegram.Bot.Domain.AggregationModels;
using Birthday.Telegram.Bot.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Birthday.Telegram.Bot.DataAccess.Repositories;

/// <inheritdoc cref="IChatMemberRepository" />
public class ChatMemberRepository : IChatMemberRepository
{
    private readonly BirthdayBotDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">Instance of current BirthdayBotDbContext</param>
    public ChatMemberRepository(BirthdayBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="CreateAsync" />
    public async Task<long> CreateAsync(ChatMember value, CancellationToken cancellationToken)
    {
        var result = await _dbContext.ChatMembers.AddAsync(value, cancellationToken);
        return result.Entity.MemberId;
    }

    /// <inheritdoc cref="UpdateAsync" />
    public Task UpdateAsync(ChatMember value, CancellationToken cancellationToken)
    {
        _dbContext.ChatMembers.Update(value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="DeleteAsync(ChatMember, CancellationToken)" />
    public Task DeleteAsync(ChatMember value, CancellationToken cancellationToken)
    {
        _dbContext.Remove(value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="DeleteAsync(long, CancellationToken)" />
    public async Task DeleteAsync(long chatMemberId, CancellationToken cancellationToken)
    {
        var chatMemberInDb = await _dbContext
                            .ChatMembers
                            .FirstOrDefaultAsync(it => it.MemberId.Equals(chatMemberId), cancellationToken);

        if (chatMemberInDb is null)
            throw new Exception($"Chat member with id {chatMemberId} not found in store");
        _dbContext.Remove(chatMemberInDb);
    }

    /// <inheritdoc cref="GetByChatMemberIdAsync" />
    public Task<ChatMember?> GetByChatMemberIdAsync(long chatMemberId, CancellationToken cancellationToken) => 
        _dbContext.ChatMembers
            .Include(it => it.GroupChatChatMembers)
            .ThenInclude(it => it.Chat)
            .FirstOrDefaultAsync(it => it.MemberId.Equals(chatMemberId), cancellationToken);
    
}
