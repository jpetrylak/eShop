using Convey;
using eShop.Application;
using eShop.Configuration;

namespace eShop;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ConfigurationManager configuration = builder.Configuration;

        builder.Services
            .AddCors()
            .AddRouting()
            .AddControllers().Services
            .ConfigureConvey()
            .AddSwaggerGen()
            .AddEntityFramework(configuration)
            .AddEmailSender(configuration)
            .AddApplicationEvents()
            .ConfigureOptions<ConfigureCorsOptions>()
            .ConfigureOptions<ConfigureSwaggerGenOptions>()
            .ConfigureOptions<ConfigureSwaggerUIOptions>()
            .ConfigureOptions<ConfigureRouteOptions>()
            .ConfigureOptions<ConfigureJsonOptions>()
            .ConfigureExceptionHandler();

        WebApplication app = builder.Build();

        app.UseExceptionHandler();
        app.UseConvey();
        app.UseCors(ConfigureCorsOptions.AllowAny);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseApplicationEvents();

        app.Run();
    }
}
