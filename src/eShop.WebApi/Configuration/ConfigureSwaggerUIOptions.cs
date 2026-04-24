using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace eShop.Configuration;

public class ConfigureSwaggerUIOptions(IWebHostEnvironment environment)
    : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        options.DocumentTitle = $"eShop {environment.EnvironmentName}";
        options.DisplayOperationId();
        options.DisplayRequestDuration();
    }
}
