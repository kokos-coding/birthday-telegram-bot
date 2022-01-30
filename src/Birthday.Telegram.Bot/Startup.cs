using Birthday.Telegram.Bot.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Birthday.Telegram.Bot
{
    /// <summary>
    /// Startup class with configurations for current project
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// IConfiguration property
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// IWebHostEnvironment property
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        /// <summary>
        /// Bot configuration
        /// </summary>
        private BotConfiguration BotConfig { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">IConfiguration instance</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment instance</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
            BotConfig = Configuration.GetSection(nameof(BotConfiguration)).Get<BotConfiguration>();
        }

        /// <summary>
        /// Method for configuration DI services for current project
        /// </summary>
        /// <param name="services">Instance of IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomOptions(Configuration)
                .AddCustomServices(Configuration);
                //.AddCustomSwagger();

            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        ///<summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        ///</summary>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="env">Instance of IWebHostEnvironment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //            app.UseHttpsRedirection();

            //app.UserCustomSwagger();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = BotConfig.AccessToken;
                endpoints.MapControllerRoute(name: "tgwebhook",
                                             pattern: $"api/{token}/update",
                                             new { controller = "Bot", action = "Post" });
                endpoints.MapControllers();
            });
        }
    }
}