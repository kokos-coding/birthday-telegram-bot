using Birthday.Telegram.Bot.Configurations;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services;
using Birthday.Telegram.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class with extensions for IServiceCollection
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    /// Configure additional IOptions params
    /// </summary>
    /// <param name="services">Instance of IServiceCollection</param>
    /// <param name="configuration">Instance of IConfiguration</param>
    /// <returns>IServiceCollection instance</returns>
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));

        return services;
    }

    /// <summary>
    /// Register services for create connection to Swagger
    /// </summary>
    /// <param name="services">Instance of IServiceCollection</param>
    /// <returns>IServiceCollection instance</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(_ => { })
            .ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
                options.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(),
                    "Birthday.Telegram.Bot.xml"));
            });

        return services;
    }

    /// <summary>
    /// Add custom HttpClients for current service
    /// </summary>
    /// <param name="services">Instance of IServiceCollection</param>
    /// <param name="configuration">IConfiguration instance</param>
    /// <returns>IServiceCollection instance</returns>
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        var botConfig = configuration.GetSection(nameof(BotConfiguration)).Get<BotConfiguration>();

        services.AddHostedService<ConfigureWebhookHostedService>();

        services.AddHttpClient();

        services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient
                        => new TelegramBotClient(botConfig.AccessToken, httpClient));

        services.AddHttpClient(Constants.TypedHttpClients.TelegramApi.ClientName,
            it => { it.BaseAddress = new Uri(Constants.TypedHttpClients.TelegramApi.Address); });

        services.AddApplicationServices();
        services.AddScoped<IBotUpdateService, BotUpdateService>();

        services.AddScoped<IBotProcessor<ChatMemberUpdated>, BotChatMemberProcessor>();
        services.AddScoped<IBotProcessor<Message>, BotMessageProcessor>();
        services.AddScoped<IBotProcessor<CallbackQuery>, BotCallbackQueryProcessor>();

        return services;
    }

    /// <summary>
    /// Добавить настройки логирования для данного проекта
    /// </summary>
    /// <param name="services">Экземпляр коллекции сервисов типа IServiceCollection</param>
    /// <returns>Экземпляр коллекции сервисов типа IServiceCollection</returns>
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        services.AddLogging(opt =>
        {
            opt.AddConsole();
#if(DEBUG)
            opt.SetMinimumLevel(LogLevel.Debug);
#else
            opt.SetMinimumLevel(LogLevel.Information);
#endif

        });

        return services;
    }
}