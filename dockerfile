FROM node:22-alpine AS web-build
WORKDIR /web

COPY Emu.Web/package*.json ./

RUN npm install

COPY Emu.Web/ ./

ARG VITE_API_BASE_URL=
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-build
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

COPY --from=dotnet-build /app/publish/api .
COPY --from=dotnet-build /app/publish/cli /app/cli
COPY --from=web-build /web/dist /app/wwwroot

COPY docker/entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh
RUN chmod +x /app/cli/Emu.Cli || true

EXPOSE 8080

ENTRYPOINT ["/entrypoint.sh"]
