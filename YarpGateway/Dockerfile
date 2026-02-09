FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["YarpGateway/YarpGateway.csproj", "YarpGateway/"]
COPY ["QuizGeneratorWebApp.ServiceDefaults/QuizGeneratorWebApp.ServiceDefaults.csproj", "QuizGeneratorWebApp.ServiceDefaults/"]
COPY ["BuildingBlocks/BuildingBlocks.Asynchronously/BuildingBlocks.Asynchronously.csproj", "BuildingBlocks/BuildingBlocks.Asynchronously/"]
COPY ["BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/BuildingBlocks.Security.csproj", "BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "YarpGateway/YarpGateway.csproj"
COPY . .
WORKDIR "/src/YarpGateway"
RUN dotnet build "./YarpGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YarpGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YarpGateway.dll"]
