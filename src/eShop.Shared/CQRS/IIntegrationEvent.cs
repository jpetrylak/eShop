using Convey.CQRS.Events;

namespace eShop.Shared.CQRS;

public interface IIntegrationEvent : IEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}