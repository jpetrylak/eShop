using Convey.MessageBrokers;
using Convey.Types;
using eShop.Shared.DDD;
using Microsoft.Extensions.Logging;

namespace eShop.Shared.CQRS;

[Decorator]
public class BusPublisherDomainEventHandlerDecorator<TDomainEvent, TIntegrationEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly ILogger<BusPublisherDomainEventHandlerDecorator<TDomainEvent, TIntegrationEvent>> _logger;
    private readonly IDomainEventHandler<TDomainEvent> _decorated;
    private readonly IBusPublisher _busPublisher;
    private readonly IDomainEventToIntegrationEventMapper _eventsMapper;

    public BusPublisherDomainEventHandlerDecorator(
        ILogger<BusPublisherDomainEventHandlerDecorator<TDomainEvent, TIntegrationEvent>> logger,
        IDomainEventHandler<TDomainEvent> decorated,
        IBusPublisher busPublisher,
        IDomainEventToIntegrationEventMapper eventsMapper)
    {
        _logger = logger;
        _decorated = decorated;
        _busPublisher = busPublisher;
        _eventsMapper = eventsMapper;
    }

    public async Task HandleAsync(TDomainEvent @event)
    {
        await _decorated.HandleAsync(@event);
        await ProcessIntegrationEventsAsync(@event);
    }

    private async Task ProcessIntegrationEventsAsync(TDomainEvent @event)
    {
        IIntegrationEvent integrationEvent = _eventsMapper.Map(@event);

        if (integrationEvent is null)
        {
            return;
        }

        _logger.LogInformation($"Publishing event '{integrationEvent.GetType().Name}' to message bus");
        await _busPublisher.PublishAsync(integrationEvent);
        _logger.LogInformation($"'{integrationEvent.GetType().Name}' was sent to to message bus");
    }
}
