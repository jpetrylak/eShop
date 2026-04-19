@echo off

dotnet ef migrations add %1 ^
-c eShop.Infrastructure.EntityFramework.EShopDbContext ^
-o EntityFramework\Migrations ^
-p ..\Source\eShop.Infrastructure\eShop.Infrastructure.csproj ^
-s ..\Source\eShop.WebApi\eShop.WebApi.csproj ^
