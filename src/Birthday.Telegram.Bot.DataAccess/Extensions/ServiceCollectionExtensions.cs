using Birthday.Telegram.Bot.DataAccess.Configurations;
using Birthday.Telegram.Bot.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Birthday.Telegram.Bot.Domain.Abstractions;
using Birthday.Telegram.Bot.Domain.AggregationModels;
using Birthday.Telegram.Bot.DataAccess;
using Birthday.Telegram.Bot.DataAccess.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class with extensions for IServiceCollection
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    /// Add service for connect to data base
    /// </summary>
    /// <param name="services">Instance of IServiceCollection</param>
    /// <param name="configuration">Instance of IConfiguration</param>
    /// <returns>Original instance of IServiceCollection</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfiguration = configuration.GetSection(nameof(DbConfiguration)).Get<DbConfiguration>();
        if(string.IsNullOrWhiteSpace(dbConfiguration.ConnectionString))
            throw new Exception($"Connection string is not defined. Please define configuration {nameof(DbConfiguration)}:{nameof(DbConfiguration.ConnectionString)}");

        services.AddDbContext<BirthdayBotDbContext>(opt => {
            opt.UseNpgsql(dbConfiguration.ConnectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMemberRepository, ChatMemberRepository>();

        return services;
    }
}
