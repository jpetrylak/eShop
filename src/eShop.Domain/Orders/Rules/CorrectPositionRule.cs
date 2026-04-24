using eShop.Domain.Products;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class CorrectPositionRule : IBusinessRule
{
    private readonly Product _product;
    private readonly int _amount;

    public CorrectPositionRule(Product product, int amount)
    {
        _product = product;
        _amount = amount;
    }

    public bool IsBroken() => _product.Id <= 0 ||
                              string.IsNullOrWhiteSpace(_product.Name) ||
                              _product.Price <= decimal.Zero ||
                              _amount <= 0;

    public string Message => string.Format(RulesMessages.InvalidPositionMessage, _product.Id, _product.Name);
    public string Code => nameof(CorrectPositionRule);
}
