param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$MigrationName
)

dotnet ef migrations add $MigrationName `
    -c eShop.Infrastructure.EntityFramework.EShopDbContext `
    -o EntityFramework\Migrations `
    -p ..\src\eShop.Infrastructure\eShop.Infrastructure.csproj `
    -s ..\src\eShop.WebApi\eShop.WebApi.csproj
