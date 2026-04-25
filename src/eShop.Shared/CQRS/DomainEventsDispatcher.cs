using eShop.Shared.DDD;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eShop.Shared.CQRS;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly ILogger<DomainEventsDispatcher> _logger;
    private readonly IServiceScopeFactory _serviceFactory;

    public DomainEventsDispatcher(ILogger<DomainEventsDispatcher> logger, IServiceScopeFactory serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        if (domainEvents is null || !domainEvents.Any())
        {
            return;
        }
        
        foreach (var @event in domainEvents)
        {
            await ProcessDomainEventAsync(@event);
        }
    }
    
    private async Task ProcessDomainEventAsync<T>(T @event) where T : IDomainEvent
    {
        using var scope = _serviceFactory.CreateScope();
        var eventType = @event.GetType();
                
        _logger.LogTrace($"Handling domain event: {eventType.Name}");
                
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        dynamic handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            _logger.LogInformation($"Handling domain event '{@event.GetType().Name}' with '{handler.GetType().Name}' handler");
            await handler.HandleAsync((dynamic) @event);
            _logger.LogInformation($"Handled domain event '{@event.GetType().Name}' with '{handler.GetType().Name}'");
        }
    }
}