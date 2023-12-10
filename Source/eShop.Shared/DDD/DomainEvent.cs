namespace eShop.Shared.DDD;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}