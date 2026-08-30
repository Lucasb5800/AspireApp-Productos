# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["AspireApp-Productos.Web/AspireApp-Productos.Web.csproj", "AspireApp-Productos.Web/"]
RUN dotnet restore "AspireApp-Productos.Web/AspireApp-Productos.Web.csproj"
COPY . .
WORKDIR "/src/AspireApp-Productos.Web"
RUN dotnet build "AspireApp-Productos.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspireApp-Productos.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspireApp-Productos.Web.dll"]