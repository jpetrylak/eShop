using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderCreatedDomainEvent(Guid OrderGuid, string UserEmail, string ShippingAddress) : DomainEventBase;