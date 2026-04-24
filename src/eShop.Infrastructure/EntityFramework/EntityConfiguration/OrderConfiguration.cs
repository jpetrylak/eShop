using eShop.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.EntityFramework.EntityConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public const int ShippingAddressMaxLength = 500;
    public const int UserEmailMaxLength = 100;
    
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Guid).IsRequired();
        builder.Property(p => p.GrandTotalValue).HasPrecision(18, 2);
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.ShippingAddress).IsRequired().HasMaxLength(ShippingAddressMaxLength);
        builder.Property(p => p.UserEmail).IsRequired().HasMaxLength(UserEmailMaxLength);
        builder.Ignore(p => p.Events);
    }
}