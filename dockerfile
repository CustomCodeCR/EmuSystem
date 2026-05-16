FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore EmuSystem.slnx

RUN dotnet publish Emu.Api/Emu.Api.csproj \
  -c Release \
  -o /app/publish \
  --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y openssl postgresql-client \
  && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
COPY docker/entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["/entrypoint.sh"]
