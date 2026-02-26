ARG DOTNET_VERSION=9.0
ARG BUILD_CONFIGURATION=Release

# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy project files first
COPY ["src/API/API.csproj", "API/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "API/API.csproj"

# Build the application
COPY ./src .
RUN dotnet build "API/API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Generate migration bundle
RUN dotnet ef migrations bundle \
    --project ./Infrastructure/Infrastructure.csproj \
    --startup-project ./API/API.csproj \
    --configuration $BUILD_CONFIGURATION \
    --self-contained -r linux-x64 -o /app/publish/migrate

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "API/API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

# Stage 3: Final Image
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh ./entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]