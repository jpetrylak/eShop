using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Events.Integration;

public record OrderCreatedIntegrationEvent(Guid OrderGuid, string UserEmail, string ShippingAddress)
    : IntegrationEventBase;