using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using eShop.Application.Orders.Commands;

namespace eShop.Application.Orders.Events.Integration.Handlers;

public class OrderCreatedIntegrationEventHandler : IEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly ICommandDispatcher _dispatcher;

    public OrderCreatedIntegrationEventHandler(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(OrderCreatedIntegrationEvent @event, CancellationToken ct)
    {
        await _dispatcher.SendAsync(new OrderCreatedNotifierCommand(@event.OrderGuid, @event.UserEmail, @event.ShippingAddress), ct);
    }
}