using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Events.Integration;

public record OrderPaidIntegrationEvent(long OrderGuid, DateTime PaymentDateTime) : IntegrationEventBase;