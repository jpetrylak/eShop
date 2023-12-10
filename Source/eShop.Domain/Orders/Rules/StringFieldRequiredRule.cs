namespace eShop.Domain.Orders.Rules;

public class StringFieldRequiredRule(string fieldName, string value) : FieldRequiredRule<string>(fieldName, value)
{
    public override bool IsBroken() => string.IsNullOrWhiteSpace(Value);
    public override string Code => nameof(StringFieldRequiredRule);
}
