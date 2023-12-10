using eShop.Domain.Orders;
using eShop.Domain.Orders.Rules;
using eShop.Domain.Products;
using eShop.Domain.Tests.Builders;
using eShop.Shared.DDD.Validation;
using FluentAssertions.Extensions;
using Shouldly;

namespace eShop.Domain.Tests.Orders;

public class MarkAsPaidTests
{
    [Fact]
    public void GivenPaidStatusOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsPaid()
            .Build();

        // act & assert
        Should.Throw<BusinessRuleException>(() => order.MarkAsPaid(1.December(2023)))
            .Code.ShouldBe(nameof(RequiredOrderStatusRule));
    }

    [Fact]
    public void GivenNoPositions_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        // act & assert
        Should.Throw<BusinessRuleException>(() => order.MarkAsPaid(1.December(2023)))
            .Code.ShouldBe(nameof(MustContainPositionRule));
    }

    [Fact]
    public void UpdatesFieldsAfterMarkingAsPaid()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .Build();

        order.AddPosition(product1, 1);

        // act
        order.MarkAsPaid(1.December(2023));

        // assert
        order.Status.ShouldBe(EOrderStatus.Paid);
        order.PaymentDateTime.ShouldBe(1.December(2023));
    }
}
