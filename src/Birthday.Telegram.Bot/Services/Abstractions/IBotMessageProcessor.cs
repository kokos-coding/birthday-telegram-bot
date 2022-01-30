using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Services.Abstractions;

/// <summary>
/// Processor for messages
/// </summary>
public interface IBotMessageProcessor : IBotProcessor<Message>
{

}
