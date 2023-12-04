using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderShippedDomainEvent(Guid OrderGuid, DateTime ShippingDateTime) : DomainEventBase;