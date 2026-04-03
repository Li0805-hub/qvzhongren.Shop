FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Shop/qvzhongren.Shop.Host/qvzhongren.Shop.Host.csproj"
RUN dotnet publish "Shop/qvzhongren.Shop.Host/qvzhongren.Shop.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "qvzhongren.Shop.Host.dll"]
