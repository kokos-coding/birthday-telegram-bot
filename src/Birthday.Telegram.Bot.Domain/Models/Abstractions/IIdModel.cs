namespace Birthday.Telegram.Bot.Domain.Models.Abstractions;

/// <summary>
/// Model with identifier
/// </summary>
/// <typeparam name="TKey">Type of identifier</typeparam>
public interface IIdModel<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Identifier of entry
    /// </summary>
    TKey Id { get; set; }
}
