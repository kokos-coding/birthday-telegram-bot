using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                    webBuilder.UseStartup<Startup>();
                });
    }
}
