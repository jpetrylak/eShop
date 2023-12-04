using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class PositionDoesNotExistsException : BusinessRuleException
{
    public PositionDoesNotExistsException()
    {
        ErrorMessage = ExceptionMessagesConsts.PositionDoesNotExistsMessage;
    }
}