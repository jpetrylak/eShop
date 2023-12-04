using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class InvalidPositionException : BusinessRuleException
{
    public InvalidPositionException(long productId, string productName)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.InvalidPositionMessage, productId, productName);
    }
}