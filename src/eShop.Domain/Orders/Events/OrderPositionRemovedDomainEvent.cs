using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderPositionRemovedDomainEvent(Guid OrderGuid, long ProductId) : DomainEventBase;