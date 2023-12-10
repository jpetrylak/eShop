using eShop.Shared.DDD;

namespace eShop.Domain.Orders.Events;

public record OrderPaidDomainEvent(long OrderId, DateTime PaymentDateTime) : DomainEventBase;