using eShop.Shared.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace eShop.Configuration;

public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        var jsonOutputFormatterMediaTypes = options
            .OutputFormatters
            .OfType<SystemTextJsonOutputFormatter>()
            .First()
            .SupportedMediaTypes;

        // Add ProblemDetails media type (application/problem+json) to the output formatters.
        // See https://tools.ietf.org/html/rfc7807
        jsonOutputFormatterMediaTypes.Insert(0, ContentType.ProblemJson);
    }
}
