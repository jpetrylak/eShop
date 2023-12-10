using eShop.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.EntityFramework.EntityConfiguration;

public class OrderPositionConfiguration : IEntityTypeConfiguration<OrderPosition>
{
    public void Configure(EntityTypeBuilder<OrderPosition> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProductName).IsRequired().HasMaxLength(ProductConfiguration.ProductNameMaxLength);
        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.UnitPrice).IsRequired().HasPrecision(18, 2);
        builder.Property(p => p.TotalValue).IsRequired().HasPrecision(18, 2);
        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.OrderId).IsRequired();
    }
}