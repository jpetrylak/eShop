using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderPositionRemovedDomainEvent(long OrderId, long ProductId) : DomainEventBase;