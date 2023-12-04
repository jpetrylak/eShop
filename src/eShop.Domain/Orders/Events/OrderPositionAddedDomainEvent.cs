using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderPositionAddedDomainEvent(Guid OrderGuid, long ProductId, string ProductName, decimal UnitPrice, int Amount)
    : DomainEventBase;