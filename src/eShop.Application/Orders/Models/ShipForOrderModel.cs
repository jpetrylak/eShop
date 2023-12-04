using System.ComponentModel.DataAnnotations;

namespace eShop.Application.Orders.Models;

public record ShipOrderModel
{
    [Required]
    [Range(1, long.MaxValue)]
    public long OrderId { get; init; }
}