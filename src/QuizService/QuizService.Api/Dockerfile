FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/QuizService/QuizService.Api/QuizService.Api.csproj", "src/QuizService/QuizService.Api/"]
COPY ["src/QuizService/QuizService.Application/QuizService.Application.csproj", "src/QuizService/QuizService.Application/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["src/QuizService/QuizService.Domain/QuizService.Domain.csproj", "src/QuizService/QuizService.Domain/"]
COPY ["BuildingBlocks/BuildingBlocks.Asynchronously/BuildingBlocks.Asynchronously.csproj", "BuildingBlocks/BuildingBlocks.Asynchronously/"]
COPY ["src/QuizService/QuizService.Infrastructure/QuizService.Infrastructure.csproj", "src/QuizService/QuizService.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/BuildingBlocks.Security.csproj", "BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security/"]
RUN dotnet restore "src/QuizService/QuizService.Api/QuizService.Api.csproj"
COPY . .
WORKDIR "/src/src/QuizService/QuizService.Api"
RUN dotnet build "./QuizService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuizService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizService.Api.dll"]
