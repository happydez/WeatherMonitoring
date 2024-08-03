#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/WeatherMonitoring.Api/WeatherMonitoring.Api.csproj", "Systems/Api/WeatherMonitoring.Api/"]
COPY ["Data/WeatherMonitoring.Context.Migrations/WeatherMonitoring.Context.Migrations.PgSql/WeatherMonitoring.Context.Migrations.PgSql.csproj", "Data/WeatherMonitoring.Context.Migrations/WeatherMonitoring.Context.Migrations.PgSql/"]
COPY ["Data/WeatherMonitoring.Context/WeatherMonitoring.Context.csproj", "Data/WeatherMonitoring.Context/"]
COPY ["Shared/WeatherMonitoring.Common/WeatherMonitoring.Common.csproj", "Shared/WeatherMonitoring.Common/"]
COPY ["Data/WeatherMonitoring.Entities/WeatherMonitoring.Entities.csproj", "Data/WeatherMonitoring.Entities/"]
COPY ["Data/WeatherMonitoring.Context.Seeder/WeatherMonitoring.Context.Seeder.csproj", "Data/WeatherMonitoring.Context.Seeder/"]
COPY ["Services/WeatherMonitoring.Services.WeatherApi/WeatherMonitoring.Services.WeatherApi.csproj", "Services/WeatherMonitoring.Services.WeatherApi/"]
COPY ["Services/WeatherMonitoring.Serivces.WeatherMonitoring/WeatherMonitoring.Services.WeatherMonitoring.csproj", "Services/WeatherMonitoring.Serivces.WeatherMonitoring/"]
COPY ["Services/WeatherMonitoring.Services.Locations/WeatherMonitoring.Services.Locations.csproj", "Services/WeatherMonitoring.Services.Locations/"]
COPY ["Services/WeatherMonitoring.Services.Weathers/WeatherMonitoring.Services.Weathers.csproj", "Services/WeatherMonitoring.Services.Weathers/"]
COPY ["Services/WeatherMonitoring.Services.Logger/WeatherMonitoring.Services.Logger.csproj", "Services/WeatherMonitoring.Services.Logger/"]
COPY ["Services/WeatherMonitoring.Services.Settings/WeatherMonitoring.Services.Settings.csproj", "Services/WeatherMonitoring.Services.Settings/"]
RUN dotnet restore "./Systems/Api/WeatherMonitoring.Api/WeatherMonitoring.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Api/WeatherMonitoring.Api"
RUN dotnet build "./WeatherMonitoring.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WeatherMonitoring.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherMonitoring.Api.dll"]