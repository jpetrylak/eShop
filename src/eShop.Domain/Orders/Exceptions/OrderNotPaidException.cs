using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class OrderNotPaidException : BusinessRuleException
{
    public OrderNotPaidException(long orderId)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.OrderNotPaidMessage, orderId);
    }
}