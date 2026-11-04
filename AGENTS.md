# Tech Challenge — Guia para agentes (`administrativo-service`)

Serviço do catálogo da oficina: cliente, veículo e serviço. Org [fiap-vcosta](https://github.com/fiap-vcosta).

## Antes de mudar código

1. Ler o README deste repositório e, quando existirem, ADRs em `docs/adrs/`
2. Não inventar features fora do escopo descrito nas rules
3. **Git:** nunca commit/push direto em `main` — branch → PR → merge (ver [`.cursor/rules/git-workflow.mdc`](.cursor/rules/git-workflow.mdc))

## Layout

| Peça | Papel |
|------|--------|
| `src/Domain` | Entities + VO `Documento` |
| `src/Application` | UseCases MediatR + gateways |
| `src/Infrastructure` | EF Core, `AddInfrastructure`, migration sem seed |
| `src/Api` | Controllers por audiência (`Staff/` + `SystemApi/`), health, Swagger |
| `nuget.config` | `building-blocks` → GitHub Packages |

Dependências apontam para dentro. Sem comentários narrativos em C#. Sem seed EF.

## Comandos

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
./scripts/setup-local-jwt.sh
dotnet run --project src/Api/Api.csproj
docker compose --profile app up --build
```
