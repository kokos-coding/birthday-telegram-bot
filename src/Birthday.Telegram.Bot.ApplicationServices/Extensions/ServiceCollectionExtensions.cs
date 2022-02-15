using MediatR;
using Birthday.Telegram.Bot.ApplicationServices.Handlers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class with extensions for IServiceCollection
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    /// Register all application services for current project
    /// </summary>
    /// <param name="services">Instance if IServiceCollection</param>
    /// <returns>Original instance of IServiceCollection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceCollectionsExtensions).Assembly);

        return services;
    }
}
