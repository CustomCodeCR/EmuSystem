FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y openssl \
  && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore VaultSecret.sln
RUN dotnet publish src/VaultSecret.Api/VaultSecret.Api.csproj \
  -c Release \
  -o /app/publish \
  --no-restore

FROM runtime AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY docker/entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["/entrypoint.sh"]
