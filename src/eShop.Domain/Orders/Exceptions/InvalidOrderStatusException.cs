using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class InvalidOrderStatusException : BusinessRuleException
{
    public InvalidOrderStatusException(EOrderStatus expected, EOrderStatus actual)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.InvalidOrderStatusMessage, expected, actual);
    }
}