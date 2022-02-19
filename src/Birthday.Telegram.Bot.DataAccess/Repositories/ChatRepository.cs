using Birthday.Telegram.Bot.Domain.AggregationModels;
using Birthday.Telegram.Bot.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Birthday.Telegram.Bot.DataAccess.Repositories;

/// <inheritdoc cref="IChatRepository" />
public class ChatRepository : IChatRepository
{
    private readonly BirthdayBotDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">Instance of current BirthdayBotDbContext</param>
    public ChatRepository(BirthdayBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="CreateAsync" />
    public async Task<long> CreateAsync(Chat value, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Chats.AddAsync(value, cancellationToken);
        return result.Entity.ChatId;
    }

    /// <inheritdoc cref="UpdateAsync" />
    public Task UpdateAsync(Chat value, CancellationToken cancellationToken)
    {
        _dbContext.Chats.Update(value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="DeleteAsync(Chat, CancellationToken)" />
    public Task DeleteAsync(Chat value, CancellationToken cancellationToken)
    {
        _dbContext.Chats.Remove(value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="DeleteAsync(long, CancellationToken)" />
    public async Task DeleteAsync(long chatId, CancellationToken cancellationToken)
    {
        var chatInDb = await _dbContext
                        .Chats
                        .FirstOrDefaultAsync(it => it.ChatId.Equals(chatId), cancellationToken);

        if (chatInDb is null)
            throw new Exception($"Chat member with id {chatId} not found in store");
        _dbContext.Remove(chatInDb);
    }

    /// <inheritdoc cref="GetByChatIdAsync" />
    public Task<Chat?> GetByChatIdAsync(long chatId, CancellationToken cancellationToken) =>
        _dbContext.Chats
            .Include(it => it.GroupChatChatMembers)
            .ThenInclude(it => it.ChatMember)
            .FirstOrDefaultAsync(it => it.ChatId.Equals(chatId), cancellationToken);

    /// <inheritdoc cref="LinkChatMembersToChatAsync" />
    public Task LinkChatMembersToChatAsync(Chat chatInfo, ICollection<ChatMember> membersInfos, CancellationToken cancellationToken)
    {
        var entriesToAdd = membersInfos.Select(it => new ChatChatMember() { ChatId = chatInfo.Id, MemberId = it.Id }).ToList();
        return _dbContext.ChatChatMembers.AddRangeAsync(entriesToAdd, cancellationToken);
    }
}
