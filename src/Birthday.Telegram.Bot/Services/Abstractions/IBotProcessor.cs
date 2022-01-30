using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public Task ProcessAsync(TAction action, CancellationToken cancellationToken);
}
