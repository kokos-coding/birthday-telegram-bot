namespace Birthday.Telegram.Bot.Services.Abstractions;

/// <summary>
/// Process for bot actions
/// </summary>
public interface IBotProcessor<TAction>
{
    /// <summary>
    /// Process action
    /// </summary>
    /// <returns></returns>
    Task ProcessAsync(TAction action, CancellationToken cancellationToken);
}
