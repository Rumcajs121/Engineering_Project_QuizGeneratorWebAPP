FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/UserService/UserService.csproj", "src/UserService/"]
COPY ["BuildingBlocks/BuildingBlocks.Asynchronously/BuildingBlocks.Asynchronously.csproj", "BuildingBlocks/BuildingBlocks.Asynchronously/"]
COPY ["BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/BuildingBlocks.Security.csproj", "BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["QuizGeneratorWebApp.ServiceDefaults/QuizGeneratorWebApp.ServiceDefaults.csproj", "QuizGeneratorWebApp.ServiceDefaults/"]
RUN dotnet restore "src/UserService/UserService.csproj"
COPY . .
WORKDIR "/src/src/UserService"
RUN dotnet build "./UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]
