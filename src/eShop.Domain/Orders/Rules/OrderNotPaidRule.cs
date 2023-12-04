using eShop.Domain.Orders.Exceptions;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class OrderNotPaidRule : IBusinessRule
{
    private readonly long _orderId;
    private readonly DateTime? _paymentDateTime;

    public OrderNotPaidRule(long orderId, DateTime? paymentDateTime)
    {
        _orderId = orderId;
        _paymentDateTime = paymentDateTime;
    }

    public bool IsBroken() => _paymentDateTime.HasValue;

    public void HandleError() => throw new OrderAlreadyPaidException(_orderId, _paymentDateTime.Value);
}