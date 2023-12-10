namespace eShop.Shared.DDD.Validation;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
    string Code { get; }
}
