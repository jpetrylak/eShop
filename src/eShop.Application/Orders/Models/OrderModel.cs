using eShop.Domain.Orders;

namespace eShop.Application.Orders.Models;

public record OrderModel
{
    public long Id { get; init; }
    public string UserEmail { get; init; }
    public EOrderStatus Status { get; init; }
    public decimal GrandTotalValue { get; init; }
    public string ShippingAddress { get; init; }
    public DateTime? ShippingDateTime { get; init; }
    public DateTime? PaymentDateTime { get; init; }
}