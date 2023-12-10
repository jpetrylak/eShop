using eShop.Domain.Orders;
using eShop.Domain.Orders.Rules;
using eShop.Shared.DDD.Validation;
using Shouldly;

namespace eShop.Domain.Tests.Orders;

public class CreateTests
{
    [Fact]
    public void GivenEmptyOrderGuid_ThrowsException()
    {
        Should.Throw<BusinessRuleException>(() => new Order(Guid.Empty, "user@eshop.com", "Polna 1"))
            .Code.ShouldBe(nameof(GuidFieldRequiredRule));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenInvalidUserEmail_ThrowsException(string userEmail)
    {
        Should.Throw<BusinessRuleException>(() => new Order(Guid.NewGuid(), userEmail, "Polna 1"))
            .Code.ShouldBe(nameof(StringFieldRequiredRule));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenInvalidShippingAddress_ThrowsException(string shippingAddress)
    {
        Should.Throw<BusinessRuleException>(() => new Order(Guid.NewGuid(), "user@eshop.com", shippingAddress))
            .Code.ShouldBe(nameof(StringFieldRequiredRule));
    }

    [Fact]
    public void CreatesOrderWithCorrectData()
    {
        // arrange
        var orderGuid = Guid.NewGuid();
        var userEmail = "user1@eshop.com";
        var shippingAddress = "Polna 1, Kraków 33-333";

        // act
        var order = new Order(orderGuid, userEmail, shippingAddress);

        // assert
        order.UserEmail.ShouldBe(userEmail);
        order.ShippingAddress.ShouldBe(shippingAddress);
        order.Guid.ShouldBe(orderGuid);
        order.Status.ShouldBe(EOrderStatus.Created);
        order.Positions.ShouldBeEmpty();
    }
}
