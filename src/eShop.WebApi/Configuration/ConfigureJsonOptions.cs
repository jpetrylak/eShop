using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace eShop.Configuration;

public class ConfigureJsonOptions :
    IConfigureOptions<Microsoft.AspNetCore.Mvc.JsonOptions>,
    IConfigureOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>
{
    public void Configure(Microsoft.AspNetCore.Mvc.JsonOptions options)
    {
        var jsonSerializerOptions = options.JsonSerializerOptions;
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        jsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    }

    public void Configure(Microsoft.AspNetCore.Http.Json.JsonOptions options)
    {
        var jsonSerializerOptions = options.SerializerOptions;
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        jsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    }
}
