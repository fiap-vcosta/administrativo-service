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
| `tests/UnitTests` | Unitários (gate Coverlet 80% line/branch por assembly) |
| `tests/IntegrationTests` | HTTP + Testcontainers Postgres |
| `terraform/` | Instância Cloud SQL própria + database/usuário/secrets; VPC via remote state do bootstrap |
| `docs/adrs/` | ADRs do serviço (ex. instância SQL própria) |
| `k8s/` | Deployment (Cloud SQL Proxy + Datadog), Service, HPA, KSA, HTTPRoute `/administrativo` |
| `.github/workflows/ci.yml` | Callers `ci-dotnet` + `ci-terraform` |
| `.github/workflows/build-push.yml` | Merge `main` → Artifact Registry |
| `.github/workflows/deploy.yml` | Manual → Secret k8s (SM + DD) + manifests + smoke `/administrativo/health` |
| `nuget.config` | `building-blocks` → GitHub Packages |

Dependências apontam para dentro. Sem comentários narrativos em C#. Sem seed EF.

## Comandos

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
dotnet test tests/UnitTests/UnitTests.csproj -c Release \
  "/p:CollectCoverage=true" \
  "/p:CoverletOutputFormat=cobertura" \
  "/p:Include=[Domain]*%2c[Application]*%2c[Infrastructure]*%2c[Api]*" \
  "/p:Threshold=80" \
  "/p:ThresholdType=line%2Cbranch" \
  "/p:ThresholdStat=minimum"
dotnet test tests/IntegrationTests/IntegrationTests.csproj -c Release
./scripts/setup-local-jwt.sh
dotnet run --project src/Api/Api.csproj
docker compose --profile app up --build
```
