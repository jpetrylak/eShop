using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class OrderAlreadyPaidException : BusinessRuleException
{
    public OrderAlreadyPaidException(long orderId, DateTime paymentDateTime)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.OrderAlreadyPaidMessage, orderId, paymentDateTime.ToHumanReadableString());
    }
}