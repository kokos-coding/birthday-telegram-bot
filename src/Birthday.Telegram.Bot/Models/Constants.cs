namespace Birthday.Telegram.Bot.Models;

/// <summary>
/// Class with constants values
/// </summary>
public static class Constants
{
    /// <summary>
    /// Bot message commands constants
    /// </summary>
    public static class BotCommands
    {
        /// <summary>
        /// Start command
        /// </summary>
        public const string Start = "/start";
    }

    /// <summary>
    /// Constants for .Net Platform
    /// </summary>
    public static class DotNetConstants
    {
        /// <summary>
        /// Well known environment variables
        /// </summary>
        public static class WellKnownEnvironments
        {
            /// <summary>
            /// Asp net core environment variable
            /// </summary>
            public const string AspNetCoreEnvironment = "ASPNETCOREENVIRONMENT";
        }
    }

    /// <summary>
    /// Names of typed https clients
    /// </summary>
    public static class TypedHttpClients
    {
        /// <summary>
        /// Telegram api http client name
        /// </summary>
        public static TypedHttpClientInformation TelegramApi = new("TelegramApi", "https://api.telegram.org");
    }
}
