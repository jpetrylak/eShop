using eShop.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.EntityFramework.EntityConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public const int ProductNameMaxLength = 500;
    
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(ProductNameMaxLength);
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
    }
}