using System;
using Birthday.Telegram.Bot.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Birthday.Telegram.Bot
{
    /// <summary>
    /// Entrypoint for current project service
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Method main
        /// </summary>
        /// <param name="args">Additional arguments for project</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Method for cheare Hot for project
        /// </summary>
        /// <param name="args">Additional arguments for project</param>
        /// <returns>Instance of hots builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(BuildConfiguration());
                    webBuilder.UseStartup<Startup>();
                });

        public static IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable(Constants.DotNetConstants.WellKnownEnvironments.AspNetCoreEnvironment);
            
            var confBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environment}.json", true)
                .AddEnvironmentVariables();
            if(environment == Environments.Development)
                confBuilder.AddUserSecrets<Startup>();

            return confBuilder.Build();
        }
    }
}
