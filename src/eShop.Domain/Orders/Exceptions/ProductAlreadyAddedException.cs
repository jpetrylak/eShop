using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class ProductAlreadyAddedException : BusinessRuleException
{
    public ProductAlreadyAddedException(string productName)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.ProductAlreadyAddedMessage, productName);
    }
}