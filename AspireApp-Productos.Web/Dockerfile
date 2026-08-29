FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AspireApp-Productos.Web/AspireApp-Productos.Web.csproj", "AspireApp-Productos.Web/"]
RUN dotnet restore "AspireApp-Productos.Web/AspireApp-Productos.Web.csproj"

# Copy everything else and publish
COPY . .
WORKDIR /src/AspireApp-Productos.Web
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and allow configuration of API base url
ENV ASPNETCORE_URLS="http://+:80"
ENV ApiBaseUrl="http://api:80"

EXPOSE 80
ENTRYPOINT ["dotnet", "AspireApp-Productos.Web.dll"]
