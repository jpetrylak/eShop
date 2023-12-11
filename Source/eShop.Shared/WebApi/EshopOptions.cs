namespace eShop.Shared.WebApi;

public record EshopOptions
{
    public const string Eshop = "Eshop";

    public bool EnableSwagger { get; init; }
}
