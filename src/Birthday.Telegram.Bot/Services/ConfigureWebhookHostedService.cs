using Birthday.Telegram.Bot.Configurations;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Services;

/// <summary>
/// Hosted service for configure webhook for current service
/// </summary>
public class ConfigureWebhookHostedService : IHostedService
{
    private readonly ILogger<ConfigureWebhookHostedService> _logger;
    private readonly IServiceProvider _services;
    private readonly BotConfiguration _botConfig;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceProvider">Instance of IServiceProvider</param>
    /// <param name="options">Instance of options for bot configuration</param>
    /// <param name="logger">Instance of ILogger</param>
    public ConfigureWebhookHostedService(IServiceProvider serviceProvider,
                            IOptions<BotConfiguration> options,
                            ILogger<ConfigureWebhookHostedService> logger)
    {
        _logger = logger;
        _services = serviceProvider;
        _botConfig = options.Value;
    }

    /// <summary>
    /// Start bot and set webhook
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Configure custom endpoint per Telegram API recommendations:
        // https://core.telegram.org/bots/api#setwebhook
        // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
        // using a secret path in the URL, e.g. https://www.example.com/<token>.
        // Since nobody else knows your bot's token, you can be pretty sure it's us.
        var webhookAddress = @$"{_botConfig.HostAddress}/api/{_botConfig.AccessToken}/update";
        _logger.LogInformation("Setting webhook: {webhookAddress}", webhookAddress);
        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Stop bot work
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook upon app shutdown
        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}