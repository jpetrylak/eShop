using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Events.Integration;

public record OrderPaidIntegrationEvent(Guid OrderGuid, DateTime PaymentDateTime) : IntegrationEventBase;