using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using eShop.Application.Orders.Commands;

namespace eShop.Application.Orders.Events.Integration.Handlers;

public class OrderShippedIntegrationEventHandler : IEventHandler<OrderShippedIntegrationEvent> 
{
    private readonly ICommandDispatcher _dispatcher;

    public OrderShippedIntegrationEventHandler(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(OrderShippedIntegrationEvent @event, CancellationToken ct)
    {
        await _dispatcher.SendAsync(new OrderShippedNotifierCommand(@event.OrderGuid, @event.ShippingDateTime), ct);
    }
}