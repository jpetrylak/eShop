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

    public string Message => string.Format(RulesMessages.OrderAlreadyPaidMessage,
        _orderId, _paymentDateTime.Value.ToHumanReadableString());
    public string Code => nameof(OrderNotPaidRule);
}
