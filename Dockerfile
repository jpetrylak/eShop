FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore
#RUN dotnet publish -c Release -o out
RUN dotnet publish ./Source/eShop.WebApi/eShop.WebApi.csproj -r linux-x64 -c Release -o /App

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App .

ENTRYPOINT ["dotnet", "eShop.WebApi.dll"]
