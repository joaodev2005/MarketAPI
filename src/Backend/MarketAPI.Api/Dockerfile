FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/MarketAPI.Api/MarketAPI.Api.csproj", "src/Backend/MarketAPI.Api/"]
COPY ["src/Shared/MarketAPI.Communication/MarketAPI.Communication.csproj", "src/Shared/MarketAPI.Communication/"]
COPY ["src/Shared/MarketAPI.Exception/MarketAPI.Exception.csproj", "src/Shared/MarketAPI.Exception/"]
COPY ["src/Backend/MarketAPI.Application/MarketAPI.Application.csproj", "src/Backend/MarketAPI.Application/"]
COPY ["src/Backend/MarketAPI.Domain/MarketAPI.Domain.csproj", "src/Backend/MarketAPI.Domain/"]
COPY ["src/Backend/MarketAPI.Infrastructure/MarketAPI.Infrastructure.csproj", "src/Backend/MarketAPI.Infrastructure/"]
RUN dotnet restore "src/Backend/MarketAPI.Api/MarketAPI.Api.csproj"
COPY . .
WORKDIR "/src/src/Backend/MarketAPI.Api"
RUN dotnet build "./MarketAPI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MarketAPI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketAPI.Api.dll"]
