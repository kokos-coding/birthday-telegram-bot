using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Runner;

namespace Birthday.Telegram.Bot.Helpers;

/// <summary>
/// Helper class with db migration
/// </summary>
public class MigrationsHelper
{
    /// <summary>
    /// Start execute migration
    /// </summary>
    /// <param name="assemblyWithMigrations">Сборка в которой хранятся миграции</param>
    /// <param name="connectionString">Строка подключения к БД</param>
    public static void RunMigrations(Assembly assemblyWithMigrations, string connectionString)
    {
        var serviceProvider = CreateServices(assemblyWithMigrations, connectionString);

        // Put the database update into a scope to ensure
        // that all resources will be disposed.
        using (var scope = serviceProvider.CreateScope())
        {
            RunMigrations(scope.ServiceProvider);
        }
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static IServiceProvider CreateServices(Assembly assemblyWithMigrations, string connectionString)
    {
        return new ServiceCollection()
            .AddLogging(opt => {
                opt.AddFluentMigratorConsole();
            })
            .AddFluentMigratorCore()
            .ConfigureRunner(opt => 
                    opt.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assemblyWithMigrations).For.All())
            .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void RunMigrations(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }
}
