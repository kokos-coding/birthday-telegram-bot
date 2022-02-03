using Birthday.Telegram.Bot.DataAccess.Configurations;
using Birthday.Telegram.Bot.Helpers;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Migrations;

namespace Birthday.Telegram.Bot;

/// <summary>
/// Entrypoint for current project service
/// </summary>
public class Program
{
    /// <summary>
    /// Method main
    /// </summary>
    /// <param name="args">Additional arguments for project</param>
    public static Task Main(string[] args) => 
        CreateHostBuilder(args)
        .Build()
        .RunAsync();
    

    /// <summary>
    /// Method for cheare Hot for project
    /// </summary>
    /// <param name="args">Additional arguments for project</param>
    /// <returns>Instance of hots builder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var configuration = BuildConfiguration();
                var dbConfiguration = configuration.GetSection(nameof(DbConfiguration)).Get<DbConfiguration>();
                MigrationsHelper.RunMigrations(typeof(InitialMigration).Assembly, dbConfiguration.ConnectionString);
                webBuilder.UseConfiguration(configuration);
                webBuilder.UseStartup<Startup>();
            });

    /// <summary>
    /// Build configuration for current project
    /// </summary>
    public static IConfiguration BuildConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable(Constants.DotNetConstants.WellKnownEnvironments.AspNetCoreEnvironment);

        var confBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environment}.json", true)
            .AddEnvironmentVariables();
        if (environment == Environments.Development)
            confBuilder.AddUserSecrets<Startup>();

        return confBuilder.Build();
    }
}

