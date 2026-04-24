using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace eShop.Configuration;

public class ConfigureCorsOptions(
    IConfiguration configuration) : IConfigureOptions<CorsOptions>
{
    public const string Default = "Default";

    public void Configure(CorsOptions options)
    {
        string[] allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        options.AddPolicy(
            Default,
            policy =>
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
        );
    }
}
