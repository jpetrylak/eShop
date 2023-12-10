using eShop.Domain.Orders;
using eShop.Domain.Orders.Rules;
using eShop.Domain.Products;
using eShop.Domain.Tests.Builders;
using eShop.Shared.DDD.Validation;
using Shouldly;

namespace eShop.Domain.Tests.Orders;

public class RemovePositionTests
{
    [Fact]
    public void GivenPaidOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsPaid()
            .Build();

        // act & assert
        Should.Throw<BusinessRuleException>(() => order.RemovePosition(1))
            .Code.ShouldBe(nameof(RequiredOrderStatusRule));
    }

    [Fact]
    public void GivenShippedOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsShipped()
            .Build();

        // act & assert
        Should.Throw<BusinessRuleException>(() => order.RemovePosition(1))
            .Code.ShouldBe(nameof(RequiredOrderStatusRule));
    }

    [Fact]
    public void GivenNotExistingPosition_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        // act & assert
        Should.Throw<BusinessRuleException>(() => order.RemovePosition(2))
            .Code.ShouldBe(nameof(PositionExistsRule));
    }

    [Fact]
    public void RemovesPositionFromCollection()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .WithName("p1")
            .WithPrice(25.25m)
            .Build();

        Product product2 = new ProductBuilder()
            .WithId(2)
            .WithName("p2")
            .WithPrice(5m)
            .Build();

        Product product3 = new ProductBuilder()
            .WithId(3)
            .WithName("p3")
            .WithPrice(10m)
            .Build();

        // act
        order.AddPosition(product1, 2);
        order.AddPosition(product2, 3);
        order.AddPosition(product3, 2);

        order.RemovePosition(1);
        int positionsCountStep1 = order.Positions.Count;
        order.RemovePosition(2);
        int positionsCountStep2 = order.Positions.Count;
        order.RemovePosition(3);
        int positionsCountStep3 = order.Positions.Count;

        // assert
        positionsCountStep1.ShouldBe(2);
        positionsCountStep2.ShouldBe(1);
        positionsCountStep3.ShouldBe(0);
    }

    [Fact]
    public void RecalculatesOrderGrandTotalValue_AfterRemovingPositions()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .WithName("p1")
            .WithPrice(25.25m)
            .Build();

        Product product2 = new ProductBuilder()
            .WithId(2)
            .WithName("p2")
            .WithPrice(5m)
            .Build();

        Product product3 = new ProductBuilder()
            .WithId(3)
            .WithName("p3")
            .WithPrice(10m)
            .Build();

        // act
        order.AddPosition(product1, 2);
        order.AddPosition(product2, 3);
        order.AddPosition(product3, 2);
        decimal grandTotalValueStep1 = order.GrandTotalValue;
        order.RemovePosition(2);
        decimal grandTotalValueStep2 = order.GrandTotalValue;
        order.RemovePosition(3);
        decimal grandTotalValueStep3 = order.GrandTotalValue;

        // assert
        grandTotalValueStep1.ShouldBe(85.50m);
        grandTotalValueStep2.ShouldBe(70.50m);
        grandTotalValueStep3.ShouldBe(50.50m);
    }
}
