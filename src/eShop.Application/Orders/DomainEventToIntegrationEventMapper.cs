using eShop.Application.Orders.Events.Integration;
using eShop.Domain.Orders.Events;
using eShop.Shared.CQRS;
using eShop.Shared.DDD;

namespace eShop.Application.Orders;

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
                    @event.OrderId,
                    @event.PaymentDateTime);
            case OrderShippedDomainEvent @event:
                return new OrderShippedIntegrationEvent(
                    @event.OrderId,
                    @event.ShippingDateTime);
            default:
                return null;
        }
    }
}
