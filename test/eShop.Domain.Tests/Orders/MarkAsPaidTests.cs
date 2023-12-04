using eShop.Domain.Orders;
using eShop.Domain.Orders.Exceptions;
using eShop.Domain.Products;
using eShop.Domain.Tests.Builders;
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
        Should.Throw<InvalidOrderStatusException>(() => order.MarkAsPaid(1.December(2023)));
    }
    
    [Fact]
    public void GivenNoPositions_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        // act & assert
        Should.Throw<OrderDoesNotContainPositionException>(() => order.MarkAsPaid(1.December(2023)));
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