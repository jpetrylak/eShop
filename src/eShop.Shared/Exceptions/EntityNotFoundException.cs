namespace eShop.Shared.Exceptions;

public class EntityNotFoundException(object entityId, Type entityType) : Exception
{
    public object EntityId { get; } = entityId;
    public Type EntityType { get; } = entityType;
}