using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using eShop.Application.Orders.Commands;

namespace eShop.Application.Orders.Events.Integration.Handlers;

public class OrderPaidIntegrationEventHandler : IEventHandler<OrderPaidIntegrationEvent> 
{
    private readonly ICommandDispatcher _dispatcher;

    public OrderPaidIntegrationEventHandler(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(OrderPaidIntegrationEvent @event, CancellationToken ct)
    {
        await _dispatcher.SendAsync(new OrderPaidNotifierCommand(@event.OrderGuid, @event.PaymentDateTime), ct);
    }
}