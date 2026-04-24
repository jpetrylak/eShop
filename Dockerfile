FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Packages.props", "."]
COPY ["nuget.config", "."]
COPY ["src/eShop.Application/", "src/eShop.Application/"]
COPY ["src/eShop.Domain/", "src/eShop.Domain/"]
COPY ["src/eShop.Infrastructure/", "src/eShop.Infrastructure/"]
COPY ["src/eShop.WebApi/", "src/eShop.WebApi/"]
COPY ["src/BuildingBlocks/eShop.Shared/", "src/BuildingBlocks/eShop.Shared/"]

RUN dotnet restore "src/eShop.WebApi/eShop.WebApi.csproj"
COPY . .
RUN dotnet build "src/eShop.WebApi/eShop.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/eShop.WebApi/eShop.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.WebApi.dll"]
