using System.Net;
using eShop.Shared.Exceptions;

namespace eShop.Shared.DDD.Validation;

[Serializable]
public class BusinessRuleException : AppException
{
    public IBusinessRule ViolatedRule { get; }
    public string ViolationDetails { get; }
    public string Code { get; }

    public BusinessRuleException(IBusinessRule violatedRule, HttpStatusCode httpStatusCode = HttpStatusCode.Conflict)
        : base("Business rule violation",
            $"{violatedRule.GetType().Name}: {violatedRule.Message}",
            httpStatusCode)
    {
        ViolatedRule = violatedRule;
        ViolationDetails = violatedRule.Message;
        Code = violatedRule.Code;
    }
}
