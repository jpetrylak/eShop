using eShop.Domain.Products;
using eShop.Shared.DDD;

namespace eShop.Domain.Orders;

public class OrderPosition : EntityBase<long>
{
    public static OrderPosition Create(long productId, string productName, decimal unitPrice, int amount, decimal totalValue)
    {
        var position = new OrderPosition
        {
            ProductId = productId,
            ProductName = productName,
            Amount = amount,
            UnitPrice = unitPrice,
            TotalValue = totalValue
        };

        return position;
    }

    public string ProductName { get; set; }
    public int Amount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
    public long OrderId { get; set; }
    public virtual Order Order { get; set; }
}