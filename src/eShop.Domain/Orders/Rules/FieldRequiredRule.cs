﻿using eShop.Domain.Orders.Exceptions;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders.Rules;

public abstract class FieldRequiredRule<TValue> : IBusinessRule
{
    protected readonly string FieldName;
    protected readonly TValue Value;

    public FieldRequiredRule(string fieldName, TValue value)
    {
        FieldName = fieldName;
        Value = value;
    }

    public virtual bool IsBroken() => EqualityComparer<TValue>.Default.Equals(Value, default);

    public void HandleError() => throw new FieldRequiredException(FieldName);
}