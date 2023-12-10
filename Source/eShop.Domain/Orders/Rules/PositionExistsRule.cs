using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class PositionExistsRule : IBusinessRule
{
    private readonly IEnumerable<OrderPosition> _positions;
    private readonly long _productId;

    public PositionExistsRule(IEnumerable<OrderPosition> positions, long productId)
    {
        _positions = positions;
        _productId = productId;
    }

    public bool IsBroken() => !_positions.Any(p => p.ProductId == _productId);
    public string Message => RulesMessages.PositionDoesNotExistsMessage;
    public string Code => nameof(PositionExistsRule);
}
