using eShop.Domain.Orders;
using eShop.Domain.Products;
using eShop.Infrastructure.EntityFramework.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.EntityFramework;

public class EShopDbContext : DbContext
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderPosition> OrderPositions => Set<OrderPosition>();
    public DbSet<OrderHistoryLog> OrderHistoryLogs => Set<OrderHistoryLog>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
    