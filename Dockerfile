# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#DNS argümanı 
ARG BUILDKIT_INLINE_CACHE=1
# Copy csproj files and restore dependencies
COPY ["API/API.csproj", "API/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Entity/Entity.csproj", "Entity/"]

RUN dotnet restore "API/API.csproj"

# Copy all source files
COPY . .

# Build and publish
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app


# Create non-root user
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Copy published app
COPY --from=build --chown=appuser:appuser /app/publish .

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5027

EXPOSE 5027

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:5027/api/category || exit 1

ENTRYPOINT ["dotnet", "API.dll"]
