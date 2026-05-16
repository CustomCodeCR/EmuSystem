FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore EmuSystem.slnx

RUN dotnet publish Emu.Api/Emu.Api.csproj \
  -c Release \
  -o /app/publish/api \
  --no-restore

RUN dotnet restore Emu.Cli/Emu.Cli.csproj -r linux-x64

RUN dotnet publish Emu.Cli/Emu.Cli.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained false \
  -o /app/publish/cli \
  --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y openssl postgresql-client \
  && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish/api .
COPY --from=build /app/publish/cli /app/cli
COPY docker/entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh
RUN chmod +x /app/cli/Emu.Cli || true

EXPOSE 8080

ENTRYPOINT ["/entrypoint.sh"]
