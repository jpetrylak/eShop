using eShop.Application.Orders.Events.Integration;
using eShop.Domain.Orders.Events;
using eShop.Shared.CQRS;
using eShop.Shared.DDD;

namespace eShop.Application;

public class DomainEventToIntegrationEventMapper : IDomainEventToIntegrationEventMapper
{
    public IIntegrationEvent Map(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case OrderCreatedDomainEvent @event:
                return new OrderCreatedIntegrationEvent(
                    @event.OrderGuid,
                    @event.UserEmail,
                    @event.ShippingAddress);
            case OrderPaidDomainEvent @event:
                return new OrderPaidIntegrationEvent(
                    @event.OrderGuid,
                    @event.PaymentDateTime);
            case OrderShippedDomainEvent @event:
                return new OrderShippedIntegrationEvent(
                    @event.OrderGuid,
                    @event.ShippingDateTime);
            default:
                return null;
        }
    }

    public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events) => events
        .Select(Map)
        .Where(e => e != null)
        .ToArray();
}