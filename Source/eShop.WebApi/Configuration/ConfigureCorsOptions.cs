using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace eShop.Configuration;

public class ConfigureCorsOptions : IConfigureOptions<CorsOptions>
{
    public const string AllowAny = "AllowAny";
    
    public void Configure(CorsOptions options) =>
        options.AddPolicy(
            AllowAny,
            x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
}
