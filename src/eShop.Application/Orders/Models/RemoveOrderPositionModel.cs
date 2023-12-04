using System.ComponentModel.DataAnnotations;

namespace eShop.Application.Orders.Models;

public record RemoveOrderPositionModel
{
    [Required]
    [Range(1, long.MaxValue)]
    public long OrderId { get; init; }
    
    [Required]
    [Range(1, long.MaxValue)]
    public long ProductId { get; init; }
}