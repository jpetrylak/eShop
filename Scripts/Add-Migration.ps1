param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$MigrationName
)

dotnet ef migrations add $MigrationName `
    -c eShop.Infrastructure.EntityFramework.EShopDbContext `
    -o EntityFramework\Migrations `
    -p ..\Source\eShop.Infrastructure\eShop.Infrastructure.csproj `
    -s ..\Source\eShop.WebApi\eShop.WebApi.csproj
