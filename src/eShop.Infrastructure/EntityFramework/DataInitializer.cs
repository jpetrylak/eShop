using eShop.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.EntityFramework;

public static class DataInitializer
{
    public static async Task SeedDataAsync(this EShopDbContext dbContext)
    {
        await SeedProductsAsync(dbContext);

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(EShopDbContext dbContext)
    {
        await SeedProductsAsync(dbContext, Product.Create("Table", 50m));
        await SeedProductsAsync(dbContext, Product.Create("Chair", 20.50m));
        await SeedProductsAsync(dbContext, Product.Create("Lamp", 7.25m));
    }

    private static async Task SeedProductsAsync(EShopDbContext dbContext, Product product)
    {
        Product existing = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == product.Name);

        if (existing is null)
        {
            dbContext.Products.Add(product);
        }
    }
}