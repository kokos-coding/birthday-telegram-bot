namespace Birthday.Telegram.Bot.Configurations;

/// <summary>
/// Configuration parameters for current bot
/// </summary>
public class BotConfiguration
{
    /// <summary>
    /// Access token for connecting to a telegram bot
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Address for hosted service
    /// </summary>
    public string HostAddress { get; set; } = string.Empty;

    /// <summary>
    /// Socks 5 host address
    /// </summary>
    public string Socks5Host { get; set; } = string.Empty;

    /// <summary>
    /// Socks 5 port
    /// </summary>
    public int Socks5Port { get; set; }
}
