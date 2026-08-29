FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AspireApp-Productos.ApiService/AspireApp-Productos.ApiService.csproj", "AspireApp-Productos.ApiService/"]
RUN dotnet restore "AspireApp-Productos.ApiService/AspireApp-Productos.ApiService.csproj"

# Copy everything else and publish
COPY . .
WORKDIR /src/AspireApp-Productos.ApiService
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and default environment for production
ENV ASPNETCORE_URLS="http://+:80"
# By default use a sqlite file inside the container; override with PRODUCTS_CONNECTION env var in production
ENV PRODUCTS_CONNECTION="Data Source=/app/products.db"

EXPOSE 80
ENTRYPOINT ["dotnet", "AspireApp-Productos.ApiService.dll"]
