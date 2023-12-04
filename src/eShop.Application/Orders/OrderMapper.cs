using eShop.Application.Orders.Models;
using eShop.Domain.Orders;

namespace eShop.Application.Orders;

public static class OrderMapper
{
    public static OrderModel Map(Order order) =>
        new()
        {
            Id = order.Id,
            GrandTotalValue = order.GrandTotalValue,
            PaymentDateTime = order.PaymentDateTime,
            ShippingAddress = order.ShippingAddress,
            ShippingDateTime = order.ShippingDateTime,
            Status = order.Status,
            Positions = Map(order.Positions)
        };

    public static IEnumerable<OrderPositionModel> Map(IEnumerable<OrderPosition> orderPositions) =>
        orderPositions
            .Select(p => new OrderPositionModel
            {
                Id = p.Id,
                ProductId = p.ProductId,
                Amount = p.Amount,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                TotalValue = p.TotalValue
            });
}