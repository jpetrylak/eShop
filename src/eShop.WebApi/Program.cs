using Convey;
using eShop.Application;
using eShop.Infrastructure;
using eShop.Shared;

namespace eShop;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;
        ConfigurationManager configuration = builder.Configuration;

        services.AddControllers();

        services
            .ConfigureConvey()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddSharedModule()
            .AddInfrastructureModule(configuration)
            .AddApplicationModule(configuration);

        WebApplication app = builder.Build();

        app.UseConvey();
        app.UseErrorHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseApplicationModule();

        app.Run();
    }
}