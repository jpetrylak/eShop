docker rm eshop-api
docker image rm eshop-api-image
cd ..
dotnet restore ./Source/eShop.WebApi/eShop.WebApi.csproj
dotnet publish ./Source/eShop.WebApi/eShop.WebApi.csproj --self-contained -c Docker -o ./App
docker build --tag eshop-api-image -f Dockerfile .
