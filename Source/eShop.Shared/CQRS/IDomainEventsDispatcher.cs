using eShop.Shared.DDD;

namespace eShop.Shared.CQRS;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events);
}