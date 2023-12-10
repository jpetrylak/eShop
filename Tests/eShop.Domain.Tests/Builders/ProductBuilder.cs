using eShop.Domain.Products;

namespace eShop.Domain.Tests.Builders;

public class ProductBuilder
{
    private long _id = 1;
    private string _name = "Chair";
    private decimal _price = 50.55m;

    public ProductBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public Product Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Price = _price
        };
}