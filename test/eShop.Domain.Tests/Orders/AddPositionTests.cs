using System.Globalization;
using eShop.Domain.Orders;
using eShop.Domain.Orders.Exceptions;
using eShop.Domain.Products;
using eShop.Domain.Tests.Builders;
using Shouldly;

namespace eShop.Domain.Tests.Orders;

public class AddPositionTests
{
    [Fact]
    public void GivenPaidOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsPaid()
            .Build();

        Product product = new ProductBuilder()
            .WithId(1)
            .Build();
        
        // act & assert
        Should.Throw<InvalidOrderStatusException>(() => order.AddPosition(product, 1));
    }
    
    [Fact]
    public void GivenShippedOrder_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsShipped()
            .Build();

        Product product = new ProductBuilder()
            .WithId(1)
            .Build();
        
        // act & assert
        Should.Throw<InvalidOrderStatusException>(() => order.AddPosition(product, 1));
    }
    
    [Theory]
    [InlineData(0, "Chair", "40.55", 2)]
    [InlineData(-2, "Chair", "40.55", 2)]
    [InlineData(1, null, "40.55", 2)]
    [InlineData(1, "", "40.55", 2)]
    [InlineData(1, "Chair", "0", 2)]
    [InlineData(1, "Chair", "-5.55", 2)]
    [InlineData(1, "Chair", "40.55", 0)]
    [InlineData(1, "Chair", "40.55", -1)]
    public void GivenInvalidPosition_ThrowsException(long productId, string productName, string productPriceStr, int amount)
    {
        // arrange
        decimal productPrice = Convert.ToDecimal(productPriceStr, CultureInfo.InvariantCulture);
        
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product = new ProductBuilder()
            .WithId(productId)
            .WithName(productName)
            .WithPrice(productPrice)
            .Build();
        
        // act & assert
        Should.Throw<InvalidPositionException>(() => order.AddPosition(product, amount));
    }
    
    [Fact]
    public void GivenDuplicatedProductPosition_ThrowsException()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .Build();

        Product product2 = new ProductBuilder()
            .WithId(1)
            .Build();
        
        // act & assert
        order.AddPosition(product1, 1);
        Should.Throw<ProductAlreadyAddedException>(() => order.AddPosition(product2, 1));
    }
    
    [Fact]
    public void AddsPositionsToCollection()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .WithName("p1")
            .WithPrice(25.55m)
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
        int positionsCountStep1 = order.Positions.Count;
        order.AddPosition(product1, 2);
        int positionsCountStep2 = order.Positions.Count;
        order.AddPosition(product2, 3);
        int positionsCountStep3 = order.Positions.Count;
        order.AddPosition(product3, 3);
        int positionsCountStep4 = order.Positions.Count;
        
        // assert
        positionsCountStep1.ShouldBe(0);
        positionsCountStep2.ShouldBe(1);
        positionsCountStep3.ShouldBe(2);
        positionsCountStep4.ShouldBe(3);
    }
    
    [Fact]
    public void AddsPositionsToCollectionWithCorrectData()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .WithName("p1")
            .WithPrice(25.55m)
            .Build();

        Product product2 = new ProductBuilder()
            .WithId(2)
            .WithName("p2")
            .WithPrice(5m)
            .Build();
        
        // act
        order.AddPosition(product1, 2);
        order.AddPosition(product2, 3);
        
        // assert
        var positins = order.Positions.ToList();
        positins.Count.ShouldBe(2);
        
        positins[0].ProductId.ShouldBe(1);
        positins[0].ProductName.ShouldBe("p1");
        positins[0].UnitPrice.ShouldBe(25.55m);
        positins[0].Amount.ShouldBe(2);
        
        positins[1].ProductId.ShouldBe(2);
        positins[1].ProductName.ShouldBe("p2");
        positins[1].UnitPrice.ShouldBe(5m);
        positins[1].Amount.ShouldBe(3);
    }
    
    [Fact]
    public void AddsPositionsToCollectionWithCorrectPositionTotalValue()
    {
        // arrange
        Order order = new OrderBuilder()
            .AsCreated()
            .Build();

        Product product1 = new ProductBuilder()
            .WithId(1)
            .WithName("p1")
            .WithPrice(25.55m)
            .Build();

        // act
        order.AddPosition(product1, 2);
        
        // assert
        var positins = order.Positions.ToList();
        positins[0].TotalValue.ShouldBe(51.1m);
    }
    
    [Fact]
    public void RecalculatesOrderGrandTotalValue_AfterAddingPositions()
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
        decimal grandTotalValueStep1 = order.GrandTotalValue;
        order.AddPosition(product2, 3);
        decimal grandTotalValueStep2 = order.GrandTotalValue;
        order.AddPosition(product3, 2);
        decimal grandTotalValueStep3 = order.GrandTotalValue;

        // assert
        grandTotalValueStep1.ShouldBe(50.5m);
        grandTotalValueStep2.ShouldBe(50.5m + 15m);
        grandTotalValueStep3.ShouldBe(50.5m + 15m + 20m);
    }
}