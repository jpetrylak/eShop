using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Exceptions;

public class FieldRequiredException : BusinessRuleException
{
    public FieldRequiredException(string fieldName)
    {
        ErrorMessage = string.Format(ExceptionMessagesConsts.FieldRequiredMessage, fieldName);
    }
}