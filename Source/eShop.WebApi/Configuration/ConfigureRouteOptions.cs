using Microsoft.Extensions.Options;

namespace eShop.Configuration;

public class ConfigureRouteOptions : IConfigureOptions<RouteOptions>
{
    public void Configure(RouteOptions options) => options.LowercaseUrls = true;
}