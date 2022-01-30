using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Services.Abstractions;

/// <summary>
/// Processor for chat member updates
/// </summary>
public interface IBotChatMemberProcessor : IBotProcessor<ChatMemberUpdated>
{

}
