using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace eShop.Configuration;

public class ConfigureSwaggerUIOptions()
    : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        options.DocumentTitle = "eShop";
        options.DisplayOperationId();
        options.DisplayRequestDuration();
    }
}