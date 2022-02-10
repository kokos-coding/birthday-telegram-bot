using Birthday.Telegram.Bot.Services.Abstractions;
using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Обработчик на Callback действия пришедшие с клавиатуры
/// </summary>
public class BotCallbackQueryProcessor : IBotProcessor<CallbackQuery>
{
    /// <inheritdoc cref="ProcessAsync" />
    public Task ProcessAsync(CallbackQuery action, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
