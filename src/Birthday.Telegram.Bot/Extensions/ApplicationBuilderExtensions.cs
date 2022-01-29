using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Class with extensions for IAppicationBuilder instance
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Extemsion for put swagger in asp new middleware
    /// </summary>
    /// <param name="builder">Instance of IApplicationBuilder</param>
    /// <returns>Instance of IApplicationBuilder</returns>
    public static IApplicationBuilder UserCustomSwagger(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Birthday telegram bot api");
                cfg.RoutePrefix = string.Empty;
            });

        return builder;
    }
}
