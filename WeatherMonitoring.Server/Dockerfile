FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs


FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WeatherMonitoring.Server/WeatherMonitoring.Server.csproj", "WeatherMonitoring.Server/"]
COPY ["weathermonitoring.client/weathermonitoring.client.esproj", "weathermonitoring.client/"]
RUN dotnet restore "./WeatherMonitoring.Server/WeatherMonitoring.Server.csproj"
COPY . .
WORKDIR "/src/WeatherMonitoring.Server"
RUN dotnet build "./WeatherMonitoring.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WeatherMonitoring.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherMonitoring.Server.dll"]
