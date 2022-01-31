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
        var result = await _dbContext.Chats.AddAsync(value);
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

    /// <inheritdoc cref="DeleteAsync(int, CancellationToken)" />
    public async Task DeleteAsync(int chatId, CancellationToken cancellationToken)
    {
        var chatInDb = await _dbContext.Chats.FirstOrDefaultAsync(it => it.ChatId.Equals(chatId));

        if (chatInDb is null)
            throw new Exception($"Chat member with id {chatId} not found in store");
        _dbContext.Remove(chatInDb);
    }

    /// <inheritdoc cref="GetByChatId" />
    public Task<Chat?> GetByChatId(int chatId, CancellationToken cancellationToken) =>
        _dbContext.Chats.FirstOrDefaultAsync(it => it.ChatId.Equals(chatId));
}
