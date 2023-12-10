namespace eShop.Application.Orders.Models;

public record OrderPositionModel
{
    public long ProductId { get; init; }
    public string ProductName { get; init; }
    public int Amount { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalValue { get; init; }
}