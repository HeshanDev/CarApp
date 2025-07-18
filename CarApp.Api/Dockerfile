# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarApp.Api/CarApp.Api.csproj", "CarApp.Api/"]
COPY ["CarApp.Application/CarApp.Application.csproj", "CarApp.Application/"]
COPY ["CarApp.Domain/CarApp.Domain.csproj", "CarApp.Domain/"]
COPY ["CarApp.Infrastructure/CarApp.Infrastructure.csproj", "CarApp.Infrastructure/"]
COPY ["CarApp.Persistence/CarApp.Persistence.csproj", "CarApp.Persistence/"]
RUN dotnet restore "./CarApp.Api/CarApp.Api.csproj"
COPY . .
WORKDIR "/src/CarApp.Api"
RUN dotnet build "./CarApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

VOLUME [ "/app/Logs" ]

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Create logs folder (optional)
RUN mkdir Logs

COPY --from=publish /app/publish .

# Expose logs directory as volume (optional)
VOLUME [ "/app/Logs" ]

ENTRYPOINT ["dotnet", "CarApp.Api.dll"]