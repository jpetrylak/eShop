using Boxed.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger.OperationFilters;
using eShop.Application;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace eShop.Configuration;

public class ConfigureSwaggerGenOptions() : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();
        options.EnableAnnotations();

        options.IncludeXmlCommentsIfExists(typeof(Program).Assembly);
        options.IncludeXmlCommentsIfExists(typeof(ApplicationModuleExtensions).Assembly);

        options.OperationFilter<ProblemDetailsOperationFilter>();
    }
}
