using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class OrderDoesNotContainPositionException : BusinessRuleException
{
    public OrderDoesNotContainPositionException(long orderId)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.OrderDoesNotContainPositionMessage, orderId);
    }
}