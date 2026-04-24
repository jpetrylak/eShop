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
    public string Message => string.Format(RulesMessages.OrderDoesNotContainPositionMessage, _orderId);
    public string Code => nameof(MustContainPositionRule);
}
