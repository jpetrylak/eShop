﻿using eShop.Shared.DDD;

namespace eShop.Shared.CQRS;

public interface IDomainEventToIntegrationEventMapper
{
    IIntegrationEvent Map(IDomainEvent @event);
    IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events);
}