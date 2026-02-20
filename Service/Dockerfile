# ─── Build Stage ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and central package/build props first (cache-friendly)
COPY Service.slnx ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY global.json ./

# Copy project files to restore dependencies (src + tests required by solution)
COPY src/Api/Api.csproj                                           src/Api/
COPY src/Application/Application.csproj                           src/Application/
COPY src/Domain/Domain.csproj                                     src/Domain/
COPY src/Infrastructure/Infrastructure.csproj                     src/Infrastructure/
COPY tests/Api.Tests/Api.Tests.csproj                             tests/Api.Tests/
COPY tests/Application.Tests/Application.Tests.csproj             tests/Application.Tests/
COPY tests/Domain.Tests/Domain.Tests.csproj                       tests/Domain.Tests/
COPY tests/Infrastructure.Tests/Infrastructure.Tests.csproj       tests/Infrastructure.Tests/

RUN dotnet restore Service.slnx

# Copy the rest of the source
COPY src/ src/

# Publish the API
RUN dotnet publish src/Api/Api.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained false \
    -o /app/publish

# ─── Runtime Stage ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Create the data directory for the SQLite database
RUN mkdir -p /data

EXPOSE 5025

CMD ["dotnet", "Api.dll"]
