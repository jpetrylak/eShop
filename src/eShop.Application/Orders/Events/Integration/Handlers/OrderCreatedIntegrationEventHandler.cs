using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Commands;
using eShop.Application.Orders.Queries;

namespace eShop.Application.Orders.Events.Integration.Handlers;

public class OrderCreatedIntegrationEventHandler : IEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly ICommandDispatcher _dispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public OrderCreatedIntegrationEventHandler(ICommandDispatcher dispatcher, IQueryDispatcher queryDispatcher)
    {
        _dispatcher = dispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public async Task HandleAsync(OrderCreatedIntegrationEvent @event, CancellationToken ct)
    {
        long orderId = await _queryDispatcher.QueryAsync(new GetOrderIdQuery(@event.OrderGuid), ct);
            
        await _dispatcher.SendAsync(new OrderCreatedNotifierCommand(orderId, @event.UserEmail, @event.ShippingAddress), ct);
    }
}