using eShop.Domain.Orders.Exceptions;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class RequiredOrderStatusRule : IBusinessRule
{
    private readonly EOrderStatus _expected;
    private readonly EOrderStatus _actual;

    public RequiredOrderStatusRule(EOrderStatus expected, EOrderStatus actual)
    {
        _expected = expected;
        _actual = actual;
    }

    public bool IsBroken() => _expected != _actual;

    public void HandleError() => throw new InvalidOrderStatusException(_expected, _actual);
}