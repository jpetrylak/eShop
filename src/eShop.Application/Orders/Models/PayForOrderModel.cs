using System.ComponentModel.DataAnnotations;

namespace eShop.Application.Orders.Models;

public record PayForOrderModel
{
    [Required]
    [Range(1, long.MaxValue)]
    public long OrderId { get; init; }
}