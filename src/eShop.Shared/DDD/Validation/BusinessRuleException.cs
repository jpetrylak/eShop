namespace eShop.Shared.DDD.Validation;

public class BusinessRuleException : Exception
{
    public string ErrorMessage { get; protected init; }
}