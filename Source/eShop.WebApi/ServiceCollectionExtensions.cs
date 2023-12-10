using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.Emailing;
using eShop.Shared.WebApi.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace eShop;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureConvey(this IServiceCollection services)
    {
        services
            .AddConvey()
            .AddRabbitMq()
            .AddCommandHandlers()
            .AddInMemoryCommandDispatcher()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddEventHandlers()
            .AddServiceBusEventDispatcher()
            .Build();

        return services;
    }

    public static IServiceCollection ConfigureExceptionHandler(this IServiceCollection services) =>
        services
            .AddExceptionHandler<AppExceptionHandler>()
            .AddProblemDetails();

    public static IServiceCollection AddEntityFramework(this IServiceCollection services,
        ConfigurationManager configuration) =>
        services
            .AddDbContext<EShopDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("eShop")))
            .AddHostedService<DatabaseInitializationTask>();

    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Smtp))
            .AddSingleton<IEmailSender, SmtpEmailSender>();
}
