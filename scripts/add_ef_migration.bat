@echo off

dotnet ef migrations add %1 ^
-c eShop.Infrastructure.EntityFramework.EShopDbContext ^
-o EntityFramework\Migrations ^
-p ..\src\eShop.Infrastructure\eShop.Infrastructure.csproj ^
-s ..\src\eShop.WebApi\eShop.WebApi.csproj ^

@REM dotnet ef migrations add Initial
@REM -c eShop.Infrastructure.EntityFramework.EShopDbContext
@REM -o src\eShop.Infrastructure\EntityFramework\Migrations
@REM -p src\eShop.Infrastructure\eShop.Infrastructure.csproj
@REM -s src\eShop.WebApi\eShop.WebApi.csproj