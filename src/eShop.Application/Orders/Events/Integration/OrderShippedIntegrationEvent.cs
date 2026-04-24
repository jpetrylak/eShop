using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Events.Integration;

public record OrderShippedIntegrationEvent(long OrderGuid, DateTime ShippingDateTime) : IntegrationEventBase;