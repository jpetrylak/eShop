namespace eShop.Shared.DDD.Validation;

public interface IBusinessRule
{
    bool IsBroken();

    void HandleError();
}