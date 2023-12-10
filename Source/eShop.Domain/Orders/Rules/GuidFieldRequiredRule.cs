namespace eShop.Domain.Orders.Rules;

public class GuidFieldRequiredRule(string fieldName, Guid value) : FieldRequiredRule<Guid>(fieldName, value)
{
    public override bool IsBroken() => Value == Guid.Empty;
    public override string Code => nameof(GuidFieldRequiredRule);
}
