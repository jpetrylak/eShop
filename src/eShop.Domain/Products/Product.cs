using eShop.Shared.DDD;

namespace eShop.Domain.Products;

public class Product : EntityBase<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public static Product Create(string name, decimal price)
    {
        return new Product
        {
            Name = name,
            Price = price
        };
    }
}