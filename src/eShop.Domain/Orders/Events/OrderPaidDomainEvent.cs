using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderPaidDomainEvent(Guid OrderGuid, DateTime PaymentDateTime) : DomainEventBase;