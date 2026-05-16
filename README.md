# Emu VaultSecret

Enterprise-ready self-hosted secret management platform built with .NET 10, PostgreSQL, JWT Authentication, API Keys, encrypted secret storage, SDK, and CLI tooling.

---

# Features

- Multi-tenant architecture
- Projects and environments
- Secret versioning
- API Keys
- Access Policies
- JWT Authentication
- CLI
- SDK
- Secret encryption
- Audit logs
- Automatic database migrations
- Automatic seed system
- Docker support
- PostgreSQL support
- Swagger/OpenAPI
- Rate limiting
- Clean Architecture
- Repository Pattern
- Unit Of Work
- EF Core 10
- Snake Case naming convention
- Automatic timestamps
- Secret rotation
- Self-hosted
- Environment isolation

---

# Tech Stack

| Technology            | Version            |
| --------------------- | ------------------ |
| .NET                  | 10                 |
| ASP.NET Core          | 10                 |
| Entity Framework Core | 10                 |
| PostgreSQL            | 16                 |
| Docker                | Latest             |
| JWT                   | Bearer             |
| OpenAPI               | Swagger            |
| CLI                   | System.CommandLine |
| SDK                   | HttpClient         |
| Encryption            | AES-256            |

---

# Architecture

```txt
EmuSystem/
├── Emu.Api
├── Emu.Application
├── Emu.Domain
├── Emu.Infrastructure
├── Emu.Sdk
├── Emu.Cli
└── docker
```

---

# Clean Architecture

## Domain

Contains:

- Entities
- Enums
- Value Objects
- Domain Events
- Interfaces

## Application

Contains:

- Features
- Commands
- Queries
- DTOs
- Abstractions
- Validators

## Infrastructure

Contains:

- EF Core
- Repositories
- Authentication
- Encryption
- Persistence
- Interceptors
- Seeders
- SDK integrations

## API

Contains:

- Minimal APIs
- Middleware
- Extensions
- Endpoint mapping
- Swagger
- Authentication

---

# Database Structure

## Main Tables

- tenants
- users
- projects
- project_environments
- secrets
- secret_versions
- api_keys
- access_policies
- audit_logs

---

# Authentication

## JWT

Used for:

- Admin access
- Dashboard access
- Swagger access
- Tenant management

## API Keys

Used for:

- Applications
- Servers
- CI/CD
- Production workloads

---

# Secret Encryption

Secrets are encrypted using:

- AES-256
- Master Key
- Per-secret encryption

Configuration:

```json
{
  "Encryption": {
    "MasterKey": "BASE64_KEY"
  }
}
```

---

# Docker

## Build

```bash
docker compose build --no-cache vaultsecret-api
```

## Start

```bash
docker compose up vaultsecret-api
```

## Start Detached

```bash
docker compose up -d vaultsecret-api
```

## Logs

```bash
docker logs -f vaultsecret-api
```

---

# Docker Runtime Features

At startup:

- Generates runtime secrets
- Waits for PostgreSQL
- Creates database automatically
- Applies EF Core migrations
- Seeds initial admin user
- Starts API

---

# Runtime Secrets

Generated automatically:

```txt
Encryption__MasterKey
ApiKeys__Pepper
Jwt__SigningKey
```

Location:

```txt
/app/secrets/vaultsecret-generated-secrets.txt
```

---

# Default Admin User

```txt
Email: admin@customcodecr.com
Password: Admin123!
```

---

# Swagger

URL:

```txt
http://localhost:8090/swagger
```

Authorize:

```txt
Bearer YOUR_TOKEN
```

---

# CLI

## CLI Location Inside Container

```txt
/app/cli/Emu.Cli
```

---

# Install CLI On Arch Linux

## Copy CLI From Container

```bash
docker cp vaultsecret-api:/app/cli ~/.local/bin/vaultsecret-cli
```

## Create Symlink

```bash
ln -sf ~/.local/bin/vaultsecret-cli/Emu.Cli ~/.local/bin/vaultsecret
```

## Add To PATH

```bash
echo 'export PATH="$HOME/.local/bin:$PATH"' >> ~/.zshrc
source ~/.zshrc
```

---

# CLI Commands

## Help

```bash
vaultsecret --help
```

---

# Authentication

## Login With JWT

```bash
vaultsecret user login \
  --base-url http://localhost:8090 \
  --tenant-id TENANT_ID \
  --email admin@customcodecr.com \
  --password Admin123!
```

## Login With API Key

```bash
vaultsecret login \
  --base-url http://localhost:8090 \
  --api-key YOUR_API_KEY
```

---

# Tenant Commands

## Create Tenant

```bash
vaultsecret tenant create \
  --name "CustomCodeCR" \
  --slug "customcodecr"
```

## List Tenants

```bash
vaultsecret tenant list
```

## Get Tenant

```bash
vaultsecret tenant get \
  --id TENANT_ID
```

---

# Project Commands

## Create Project

```bash
vaultsecret project create \
  --tenant-id TENANT_ID \
  --name "Dhole" \
  --slug "dhole"
```

## List Projects

```bash
vaultsecret project list \
  --tenant-id TENANT_ID
```

---

# Environment Commands

## Create Environment

```bash
vaultsecret env create \
  --project-id PROJECT_ID \
  --name "Production" \
  --slug "prod"
```

## List Environments

```bash
vaultsecret env list \
  --project-id PROJECT_ID
```

---

# API Key Commands

## Create API Key

```bash
vaultsecret api-key create \
  --tenant-id TENANT_ID \
  --name "Production API"
```

## List API Keys

```bash
vaultsecret api-key list \
  --tenant-id TENANT_ID
```

## Disable API Key

```bash
vaultsecret api-key disable \
  --id API_KEY_ID
```

---

# Policy Commands

## Create Policy

```bash
vaultsecret policy create \
  --api-key-id API_KEY_ID \
  --tenant-id TENANT_ID \
  --path-prefix "database/" \
  --read true \
  --write true \
  --delete false
```

## List Policies

```bash
vaultsecret policy list \
  --api-key-id API_KEY_ID
```

---

# Secret Commands

## Create Secret

```bash
vaultsecret secret set \
  --environment-id ENVIRONMENT_ID \
  --name "postgres-password" \
  --path "database/postgres/password" \
  --value "SuperSecret123"
```

## Get Secret

```bash
vaultsecret secret get \
  --environment-id ENVIRONMENT_ID \
  --path "database/postgres/password"
```

## List Secrets

```bash
vaultsecret secret list \
  --environment-id ENVIRONMENT_ID
```

## Rotate Secret

```bash
vaultsecret secret rotate \
  --id SECRET_ID \
  --value "NewSecretValue"
```

## Delete Secret

```bash
vaultsecret secret delete \
  --id SECRET_ID
```

---

# User Commands

## Create User

```bash
vaultsecret user create \
  --tenant-id TENANT_ID \
  --email user@example.com \
  --full-name "Example User" \
  --password "StrongPassword123!"
```

## List Users

```bash
vaultsecret user list \
  --tenant-id TENANT_ID
```

---

# Audit Log Commands

## List Audit Logs

```bash
vaultsecret audit-log list \
  --tenant-id TENANT_ID
```

---

# SDK

## Dependency Injection

```csharp
services.AddVaultSecretClient(configuration);
```

---

# SDK Configuration

```json
{
  "VaultSecret": {
    "BaseUrl": "http://localhost:8090",
    "ApiKey": "YOUR_API_KEY"
  }
}
```

---

# SDK Usage

```csharp
var secret = await client.GetSecretByPathAsync(
    environmentId,
    "database/postgres/password");
```

---

# EF Core Migrations

## Create Migration

```bash
dotnet ef migrations add InitialCreate \
  --project Emu.Infrastructure/Emu.Infrastructure.csproj \
  --startup-project Emu.Api/Emu.Api.csproj \
  --context ApplicationDbContext \
  --output-dir Persistence/Migrations
```

## Update Database

```bash
dotnet ef database update \
  --project Emu.Infrastructure/Emu.Infrastructure.csproj \
  --startup-project Emu.Api/Emu.Api.csproj
```

---

# Security

- JWT Authentication
- API Key Authentication
- AES-256 Secret Encryption
- Secret Isolation
- Per-environment permissions
- Audit Logging
- Automatic timestamps
- Rate limiting
- Secret rotation
- Scoped policies

---

# Rate Limiting

Configured groups:

- auth
- api
- secrets-read

---

# Automatic Seed

Creates automatically:

- Default tenant
- Default admin user

---

# Naming Convention

Database naming:

```txt
snake_case
```

Examples:

```txt
tenant_id
created_at
secret_versions
```

---

# API Routes

## Auth

```txt
/api/auth/login
```

## Tenants

```txt
/api/tenants
```

## Projects

```txt
/api/projects
```

## Environments

```txt
/api/environments
```

## Secrets

```txt
/api/secrets
```

## API Keys

```txt
/api/api-keys
```

## Policies

```txt
/api/policies
```

## Audit Logs

```txt
/api/audit-logs
```

---

# Future Roadmap

- UI Dashboard
- Secret Expiration
- Secret Replication
- Kubernetes Operator
- Hashicorp Vault compatibility
- Secret Leasing
- Dynamic Secrets
- Multi-region support
- Secret templates
- Webhooks
- Redis caching
- S3 backups
- HSM support
- Multi-factor authentication
- RBAC
- Organization hierarchy
- Secret sync agent
- GitOps integration
- Terraform provider
- OpenTelemetry
- Metrics dashboard

---

# License

CustomCodeCR Internal License

---

# Author

CustomCodeCR

Maurice Lang
