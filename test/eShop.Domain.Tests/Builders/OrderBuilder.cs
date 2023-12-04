using eShop.Domain.Orders;
using eShop.Domain.Products;
using FluentAssertions.Extensions;

namespace eShop.Domain.Tests.Builders;

public class OrderBuilder
{
    private long _id = 1;
    private Guid _orderGuid = Guid.NewGuid();
    private string _userEmail = "user@eshop.com";
    private string _shippingAddress = "Polna 1, Kraków 33-333";
    private DateTime _paymentDateTime = 1.December(2023);
    private DateTime _shippingDateTime = 2.December(2023);
    private EOrderStatus _expectedOrderStatus;

    public OrderBuilder WithId(long id)
    {
        _id = id;
        return this;
    }
    
    public OrderBuilder WithUserEmail(string userEmail)
    {
        _userEmail = userEmail;
        return this;
    }
    
    public OrderBuilder WithShippingAddress(string shippingAddress)
    {
        _shippingAddress = shippingAddress;
        return this;
    }
    
    public OrderBuilder WithPaymentDate(DateTime paymentDateTime)
    {
        _paymentDateTime = paymentDateTime;
        return this;
    }
    
    public OrderBuilder WithShippingDate(DateTime shippingDateTime)
    {
        _shippingDateTime = shippingDateTime;
        return this;
    }
    
    public OrderBuilder AsCreated()
    {
        _expectedOrderStatus = EOrderStatus.Created;
        return this;
    }
    
    public OrderBuilder AsPaid()
    {
        _expectedOrderStatus = EOrderStatus.Paid;
        return this;
    }

    public OrderBuilder AsShipped()
    {
        _expectedOrderStatus = EOrderStatus.Shipped;
        return this;
    }
    
    public Order Build()
    {
        var order = new Order(_orderGuid, _userEmail, _shippingAddress);
        order.Id = _id;
        
        switch (_expectedOrderStatus)
        {
            case EOrderStatus.Paid:
                MakeOrderPaid(order);
                break;
            case EOrderStatus.Shipped:
                MakeOrderPaid(order);
                order.MarkAsShipped(_shippingDateTime);
                break;
        }

        return order;
    }

    private void MakeOrderPaid(Order order)
    {
        Product product = new ProductBuilder()
            .WithId(1)
            .Build();
        
        order.AddPosition(product, 1);
        order.MarkAsPaid(_paymentDateTime);
    }
}