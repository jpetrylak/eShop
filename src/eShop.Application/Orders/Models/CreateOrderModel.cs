using System.ComponentModel.DataAnnotations;
using eShop.Infrastructure.EntityFramework.EntityConfiguration;

namespace eShop.Application.Orders.Models;

public record CreateOrderModel
{
    [Required]
    [StringLength(OrderConfiguration.UserEmailMaxLength)]
    [EmailAddress]
    public string UserEmail { get; init; }
    
    [Required]
    [StringLength(OrderConfiguration.ShippingAddressMaxLength)]
    public string ShippingAddress { get; init; }

    public IEnumerable<CreateOrderPositionModel> Positions { get; init; }
    
}