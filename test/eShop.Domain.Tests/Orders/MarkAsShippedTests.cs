using eShop.Domain.Orders;
using eShop.Domain.Orders.Exceptions;
using eShop.Domain.Tests.Builders;
using FluentAssertions.Extensions;
using Shouldly;

namespace eShop.Domain.Tests.Orders;

public class MarkAsShippedTests
{
    [Fact]
    public void GivenShippedOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsShipped()
            .Build();

        // act & assert
        Should.Throw<InvalidOrderStatusException>(() => order.MarkAsShipped(1.December(2023)));
    }
    
    [Fact]
    public void GivenNotPaidOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsShipped()
            .Build();

        // act & assert
        Should.Throw<InvalidOrderStatusException>(() => order.MarkAsShipped(1.December(2023)));
    }
    
    [Fact]
    public void UpdatesFieldsAfterMarkingAsShipped()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsPaid()
            .Build();

        // act
        order.MarkAsShipped(2.December(2023));
        
        // assert
        order.Status.ShouldBe(EOrderStatus.Shipped);
        order.ShippingDateTime.ShouldBe(2.December(2023));
    }
}