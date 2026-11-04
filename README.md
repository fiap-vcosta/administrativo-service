# administrativo-service

Serviço do catálogo da oficina: cliente, veículo e serviço. Expõe a API pública do administrativo e uma API interna para os outros serviços consultarem esses dados.

Stack: .NET 8 e PostgreSQL. Org [fiap-vcosta](https://github.com/fiap-vcosta). Pacote compartilhado: [`building-blocks`](https://github.com/fiap-vcosta/building-blocks) (GitHub Packages; `nuget.config` já mapeia o feed).

Requisitos: [docs/01_requisitos.md](docs/01_requisitos.md).

## Layout

| Projeto | Papel |
|---------|--------|
| `src/Domain` | Cliente, Veículo, Serviço (catálogo) |
| `src/Application` | Use cases MediatR + ports de Gateway |
| `src/Infrastructure` | EF Core (PostgreSQL), gateways, migration inicial **sem seed** |

Api, Docker, testes, Terraform e k8s entram nos PRs seguintes do template.

## Dados de demo

Não há seed no banco. Cadastros de cliente/veículo/serviço são feitos via Requestly (e depois e2e).

Connection string de design-time / exemplo usa `Maximum Pool Size=15`.

## Build

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
```

Migration:

```bash
dotnet ef migrations add <Nome> \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/Infrastructure/Infrastructure.csproj \
  --output-dir Database/Migrations
```
