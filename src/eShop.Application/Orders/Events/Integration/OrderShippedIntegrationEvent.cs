using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Events.Integration;

public record OrderShippedIntegrationEvent(Guid OrderGuid, DateTime ShippingDateTime) : IntegrationEventBase;