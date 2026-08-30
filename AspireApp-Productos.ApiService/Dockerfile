# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["AspireApp-Productos.ApiService/AspireApp-Productos.ApiService.csproj", "AspireApp-Productos.ApiService/"]
RUN dotnet restore "AspireApp-Productos.ApiService/AspireApp-Productos.ApiService.csproj"
COPY . .
WORKDIR "/src/AspireApp-Productos.ApiService"
RUN dotnet build "AspireApp-Productos.ApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspireApp-Productos.ApiService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspireApp-Productos.ApiService.dll"]