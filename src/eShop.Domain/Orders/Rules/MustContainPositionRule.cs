using eShop.Domain.Orders.Exceptions;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class MustContainPositionRule : IBusinessRule
{
    private readonly IEnumerable<OrderPosition> _positions;
    private long _orderId;

    public MustContainPositionRule(long orderId, IEnumerable<OrderPosition> positions)
    {
        _positions = positions;
        _orderId = orderId;
    }

    public bool IsBroken() => _positions == null || !_positions.Any();

    public void HandleError() => throw new OrderDoesNotContainPositionException(_orderId);
}