using eShop.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.EntityFramework.EntityConfiguration;

public class OrderHistoryLogConfiguration : IEntityTypeConfiguration<OrderHistoryLog>
{
    public const int MessageMaxLength = 1000;
    
    public void Configure(EntityTypeBuilder<OrderHistoryLog> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Message).IsRequired().HasMaxLength(MessageMaxLength);
        builder.Property(p => p.Occured).IsRequired();
        builder.Property(p => p.OrderId).IsRequired();
    }
}