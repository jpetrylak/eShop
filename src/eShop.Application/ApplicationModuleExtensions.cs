using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using eShop.Application.Orders.Events;
using eShop.Application.Orders.Events.Integration;
using eShop.Domain.Orders.Events;
using eShop.Shared.CQRS;
using eShop.Shared.DDD;
using eShop.Shared.Emailing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace eShop.Application;

public static class ApplicationModuleExtensions
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddEmailSender(configuration)
            .AddEvents();

        return services;
    }

    public static IApplicationBuilder UseApplicationModule(this IApplicationBuilder app)
    {
        app.UseRabbitMq()
            .SubscribeEvent<OrderCreatedIntegrationEvent>()
            .SubscribeEvent<OrderPaidIntegrationEvent>()
            .SubscribeEvent<OrderShippedIntegrationEvent>();

        return app;
    }

    private static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services
            .AddSingleton<IDomainEventToIntegrationEventMapper, DomainEventToIntegrationEventMapper>()
            .AddTransient<IDomainEventHandler<OrderCreatedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPaidDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPositionAddedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPositionRemovedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderShippedDomainEvent>, OrderHistoryLogHandler>()
            .DecorateDomainEventWithBusPublisher<OrderCreatedDomainEvent, OrderCreatedIntegrationEvent>()
            .DecorateDomainEventWithBusPublisher<OrderPaidDomainEvent, OrderPaidIntegrationEvent>()
            .DecorateDomainEventWithBusPublisher<OrderShippedDomainEvent, OrderShippedIntegrationEvent>();

        return services;
    }

    private static IServiceCollection
        DecorateDomainEventWithBusPublisher<TDomainEvent, TIntegrationEvent>(this IServiceCollection services)
        where TDomainEvent : IDomainEvent
    {
        services
            .Decorate<IDomainEventHandler<TDomainEvent>,
                BusPublisherDomainEventHandlerDecorator<TDomainEvent, TIntegrationEvent>>();

        return services;
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Smtp))
            .AddSingleton<IEmailSender, SmtpEmailSender>();

        return services;
    }
}