using Birthday.Telegram.Bot.Domain.Models.Abstractions;

namespace Birthday.Telegram.Bot.Domain.Models;

/// <inheritdoc />
public class IdModel<TKey> : IIdModel<TKey>
    where TKey : IEquatable<TKey>
{
    /// <inheritdoc cref="Id" />
    public TKey Id { get; set; }
}