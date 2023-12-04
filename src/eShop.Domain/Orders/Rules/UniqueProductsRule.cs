using eShop.Domain.Orders.Exceptions;
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

    public void HandleError() => throw new ProductAlreadyAddedException(_productName);
}