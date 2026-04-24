namespace eShop.Application.Orders.Models;

public record OrderHistoryModel
{
    public string Message { get; init; }
    public DateTime Occured { get; init; }
}