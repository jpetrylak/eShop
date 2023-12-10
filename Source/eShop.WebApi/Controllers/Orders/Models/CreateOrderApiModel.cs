using System.ComponentModel.DataAnnotations;
using eShop.Infrastructure.EntityFramework.EntityConfiguration;

namespace eShop.Controllers.Orders.Models;

public record CreateOrderApiModel
{
    /// <summary>
    /// Gets or sets the order user e-mail.
    /// </summary>
    /// <example>user@eshop.com</example>
    [Required]
    [StringLength(OrderConfiguration.UserEmailMaxLength)]
    [EmailAddress]
    public string UserEmail { get; init; }
    
    /// <summary>
    /// Gets or sets the order shipping address.
    /// </summary>
    /// <example>Polna 1, 30-300 Kraków</example>
    [Required]
    [StringLength(OrderConfiguration.ShippingAddressMaxLength)]
    public string ShippingAddress { get; init; }
}