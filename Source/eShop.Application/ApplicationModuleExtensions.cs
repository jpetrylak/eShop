using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using eShop.Application.Orders;
using eShop.Application.Orders.Events;
using eShop.Application.Orders.Events.Integration;
using eShop.Domain.Orders.Events;
using eShop.Shared.CQRS;
using eShop.Shared.DDD;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Application;

public static class ApplicationModuleExtensions
{
    public static IServiceCollection AddApplicationEvents(this IServiceCollection services) =>
        services
            .AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>()
            .AddSingleton<IDomainEventToIntegrationEventMapper, DomainEventToIntegrationEventMapper>()
            .AddTransient<IDomainEventHandler<OrderCreatedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPaidDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPositionAddedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderPositionRemovedDomainEvent>, OrderHistoryLogHandler>()
            .AddTransient<IDomainEventHandler<OrderShippedDomainEvent>, OrderHistoryLogHandler>()
            .DecorateDomainEventWithBusPublisher<OrderCreatedDomainEvent, OrderCreatedIntegrationEvent>()
            .DecorateDomainEventWithBusPublisher<OrderPaidDomainEvent, OrderPaidIntegrationEvent>()
            .DecorateDomainEventWithBusPublisher<OrderShippedDomainEvent, OrderShippedIntegrationEvent>();

    public static IApplicationBuilder UseApplicationEvents(this IApplicationBuilder app)
    {
        app.UseRabbitMq()
            .SubscribeEvent<OrderCreatedIntegrationEvent>()
            .SubscribeEvent<OrderPaidIntegrationEvent>()
            .SubscribeEvent<OrderShippedIntegrationEvent>();

        return app;
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
}
