using System.Text.Json.Serialization;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using eShop.Shared.CQRS;
using eShop.Shared.DDD.Validation;
using eShop.Shared.Exceptions;
using eShop.Shared.WebApi.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Shared;

public static class SharedModuleExtensions
{
    public static IServiceCollection AddSharedModule(this IServiceCollection services) =>
        services
            .ConfigureStringEnumOptions()
            .AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>()
            .AddErrorHandling();

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
    
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
        => app.UseExceptionHandler(_ => { });

    private static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services
            .AddSingleton<IExceptionHandler<EntityNotFoundException>, EntityNotFoundExceptionHandler>()
            .AddSingleton<IExceptionHandler<BusinessRuleException>, BusinessRuleExceptionHandler>()
            .AddExceptionHandler<EShopExceptionHandler>();

        return services;
    }

    private static IServiceCollection ConfigureStringEnumOptions(this IServiceCollection services)
    {
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
                o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(o =>
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}