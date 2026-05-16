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
