namespace Birthday.Telegram.Bot.Models;

// <summary>
/// Record that represent a typed http client
/// </summary>
public record TypedHttpClientInformation(string ClientName, string Address)
{
    /// <summary>
    ///     Get string from client
    /// </summary>
    /// <returns>Http client name</returns>
    public override string ToString()
    {
        return ClientName;
    }
}
