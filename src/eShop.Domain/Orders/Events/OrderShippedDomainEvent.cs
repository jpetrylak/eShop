using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderShippedDomainEvent(long OrderId, DateTime ShippingDateTime) : DomainEventBase;