using System.ComponentModel.DataAnnotations;

namespace eShop.Controllers.Orders.Models;

public record CreateOrderPositionApiModel
{
    [Required]
    [Range(1, long.MaxValue)]
    public long ProductId { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Amount { get; init; }
}