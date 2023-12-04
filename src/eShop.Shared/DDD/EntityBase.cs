namespace eShop.Shared.DDD;

public abstract class EntityBase<T>
{
    public T Id { get; set; }
}