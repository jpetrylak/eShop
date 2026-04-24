using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public class OrderPaidRule : IBusinessRule
{
    private readonly long _orderId;
    private readonly DateTime? _paymentDateTime;

    public OrderPaidRule(long orderId, DateTime? paymentDateTime)
    {
        _orderId = orderId;
        _paymentDateTime = paymentDateTime;
    }

    public bool IsBroken() => !_paymentDateTime.HasValue;
    public string Message => string.Format(RulesMessages.OrderNotPaidMessage, _orderId);
    public string Code => nameof(OrderPaidRule);
}
