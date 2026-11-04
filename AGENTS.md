# Tech Challenge — Guia para agentes (`administrativo-service`)

Serviço do catálogo da oficina: cliente, veículo e serviço. Org [fiap-vcosta](https://github.com/fiap-vcosta).

## Antes de mudar código

1. Ler o README deste repositório e, quando existirem, ADRs em `docs/adrs/`
2. Não inventar features fora do escopo descrito nas rules
3. **Git:** nunca commit/push direto em `main` — branch → PR → merge (ver [`.cursor/rules/git-workflow.mdc`](.cursor/rules/git-workflow.mdc))

## Layout

| Peça | Papel |
|------|--------|
| `src/Domain` | Entities Cliente / Veículo / Serviço + `DocumentoNormalizer` |
| `src/Application` | UseCases MediatR + `IClienteGateway` / `IVeiculoGateway` / `IServicoGateway` |
| `src/Infrastructure` | EF Core, gateways, `AddInfrastructure`, migration sem seed |
| `nuget.config` | `building-blocks` → GitHub Packages (consumo na Api) |

Dependências apontam para dentro. Sem comentários narrativos em C#.

## Fora de escopo

Ordem de serviço, estoque, orçamento e emissão de JWT. Sem seed EF (`HasData`); dados via Requestly/e2e.

## Comandos

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
dotnet ef migrations add <Nome> \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/Infrastructure/Infrastructure.csproj \
  --output-dir Database/Migrations
```
