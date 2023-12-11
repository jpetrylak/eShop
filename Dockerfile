FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Source/eShop.Application/", "eShop.Application/"]
COPY ["Source/eShop.Domain/", "eShop.Domain/"]
COPY ["Source/eShop.Infrastructure/", "eShop.Infrastructure/"]
COPY ["Source/eShop.Shared/", "eShop.Shared/"]
COPY ["Source/eShop.WebApi/", "eShop.WebApi/"]

RUN dotnet restore "eShop.WebApi/eShop.WebApi.csproj"
COPY . .
RUN dotnet build "eShop.WebApi/eShop.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eShop.WebApi/eShop.WebApi.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.WebApi.dll"]

