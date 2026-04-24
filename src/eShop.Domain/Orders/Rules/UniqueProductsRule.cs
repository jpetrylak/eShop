using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class UniqueProductsRule : IBusinessRule
{
    private readonly IEnumerable<OrderPosition> _positions;
    private readonly long _productId;
    private readonly string _productName;

    public UniqueProductsRule(IEnumerable<OrderPosition> positions, long productId, string productName)
    {
        _positions = positions;
        _productId = productId;
        _productName = productName;
    }

    public bool IsBroken() => _positions.Any(p => p.ProductId == _productId);
    public string Message => string.Format(RulesMessages.ProductAlreadyAddedMessage, _productName);
    public string Code => nameof(UniqueProductsRule);
}
