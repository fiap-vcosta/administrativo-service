# administrativo-service

[![Quality Gate](https://sonarcloud.io/api/project_badges/quality_gate?project=fiap-vcosta_administrativo-service)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=fiap-vcosta_administrativo-service&metric=coverage)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)
[![Reliability](https://sonarcloud.io/api/project_badges/measure?project=fiap-vcosta_administrativo-service&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)
[![Security](https://sonarcloud.io/api/project_badges/measure?project=fiap-vcosta_administrativo-service&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)
[![Maintainability](https://sonarcloud.io/api/project_badges/measure?project=fiap-vcosta_administrativo-service&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)
[![Duplications](https://sonarcloud.io/api/project_badges/measure?project=fiap-vcosta_administrativo-service&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=fiap-vcosta_administrativo-service)

Serviço do catálogo da oficina: cliente, veículo e serviço. Expõe a API pública do administrativo e uma API interna para os outros serviços consultarem esses dados.

Stack: .NET 8 e PostgreSQL. Org [fiap-vcosta](https://github.com/fiap-vcosta). Pacote compartilhado: [`building-blocks`](https://github.com/fiap-vcosta/building-blocks) **1.0.0** (GitHub Packages).

- Requisitos: [`docs/01_requisitos.md`](docs/01_requisitos.md)
- Domínio (linguagem + modelo): [`e2e` / docs/dominio/administrativo](https://github.com/fiap-vcosta/e2e/blob/main/docs/dominio/administrativo.md)
- Cloud SQL 1/serviço: [`e2e` ADR 001](https://github.com/fiap-vcosta/e2e/blob/main/docs/adrs/001-instancia-cloud-sql-por-servico.md)

## Layout

| Projeto | Papel |
|---------|--------|
| `src/Domain` | Cliente, Veículo, Serviço + VO `Documento` |
| `src/Application` | Use cases MediatR + gateways |
| `src/Infrastructure` | EF Core (PostgreSQL), migration **sem seed** |
| `src/Api` | Controllers `Staff/` + `SystemApi/` (`/administrativo/…` e `/system/administrativo/…`) |
| `tests/UnitTests` | Gate Coverlet ≥ 80% line/branch |
| `tests/IntegrationTests` | HTTP + Testcontainers Postgres |
| `terraform/` | Database `administrativo`, usuário com role `administrativo_app`, secrets SM |
| `k8s/` | Deployment (Cloud SQL Proxy + Datadog), Service, HPA, KSA, HTTPRoute `/administrativo` |
| `docs/requestly/` | Collection Requestly (Docker / Local / GCP) |

## Dados de demo

Não há seed. Cadastros via Requestly ([docs/requestly/](docs/requestly/)) ou e2e. Connection string usa `Maximum Pool Size=15`.

## JWT local (teste)

Chave privada **não** vai para o Git.

```bash
./scripts/setup-local-jwt.sh   # cria .local/jwt/*.pem e esboço de .env
./scripts/mint-staff-jwt.sh    # imprime Bearer JWT RS256 (cole em Requestly → token)
```

Em `Development`, a Api carrega `.local/jwt/public.pem` automaticamente se o arquivo existir. No Compose, use `JWT_PUBLIC_KEY_PEM` no `.env` (o setup script preenche).

## Local

```bash
# PAT read:packages para restore / docker build
export NUGET_AUTH_TOKEN=…   # ou autentique o feed github no NuGet

dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
./scripts/setup-local-jwt.sh
dotnet run --project src/Api/Api.csproj
# → http://localhost:8080/administrativo/health
# Swagger: http://localhost:8080/swagger
```

A app sempre lê `ConnectionStrings__DefaultConnection` (local, Compose `api` e Secret no GKE). `POSTGRES_*` existe só para o container Postgres no Compose.

Compose (app + Postgres):

```bash
cp .env.example .env   # ou use o .env gerado pelo setup-local-jwt
# preencha NUGET_AUTH_TOKEN
docker compose --profile app up --build
# → http://localhost:8080/administrativo/health
# Swagger: http://localhost:8080/swagger
```

Requestly deste serviço: [`docs/requestly/`](docs/requestly/). Sistema: [`e2e/docs/requestly/`](https://github.com/fiap-vcosta/e2e/tree/main/docs/requestly).

## Testes

```bash
dotnet test tests/UnitTests/UnitTests.csproj -c Release \
  "/p:CollectCoverage=true" \
  "/p:CoverletOutputFormat=cobertura" \
  "/p:Include=[Domain]*%2c[Application]*%2c[Infrastructure]*%2c[Api]*" \
  "/p:Threshold=80" \
  "/p:ThresholdType=line%2Cbranch" \
  "/p:ThresholdStat=minimum"
# Integração precisa de Docker (Testcontainers Postgres)
dotnet test tests/IntegrationTests/IntegrationTests.csproj -c Release
```

## CI

Caller dos reusables em [`fiap-vcosta/actions`](https://github.com/fiap-vcosta/actions):

- **dotnet** — InspectCode, unit (gate 80%), integração, SonarCloud (`fiap-vcosta_administrativo-service`); restore do `building-blocks` via GitHub Packages (`packages: read`)
- **terraform** — `fmt` / `validate` no root `terraform/` (plan fica para o `tf-apply`)

Secret necessário no repo: `SONAR_TOKEN`.

## Terraform (GCP)

Root module: [`terraform/`](terraform/). State GCS prefix `administrativo-service`. Cria a **própria** instância Cloud SQL; a VPC vem do state do [`infra-bootstrap`](https://github.com/fiap-vcosta/infra-bootstrap). Decisão de sistema: [`e2e` ADR 001](https://github.com/fiap-vcosta/e2e/blob/main/docs/adrs/001-instancia-cloud-sql-por-servico.md) (1 instância por serviço SQL).

| Recurso | Papel |
|---------|--------|
| Instância `administrativo-service-pg` | Cloud SQL PostgreSQL 16 (`db-f1-micro`, IP privado) |
| Database `administrativo` | Banco do serviço |
| Usuário `administrativo` | Login da app (grants de schema via Data API) |
| Secrets SM | `administrativo-service-connection-string` (`Maximum Pool Size=4`) |
| Org secret | `SERVICE_AUTH_KEY` — shared secret da malha (`X-Service-Key` / `ServiceAuth__Key`) |

`tf-apply` / `tf-destroy` / `deploy` são **manuais de propósito** (custo da janela GCP). CI e `build-push` seguem automáticos. Narrativa: [`e2e` ADR 006](https://github.com/fiap-vcosta/e2e/blob/main/docs/adrs/006-deploy-demo-manuais-custo.md).

O `deploy` monta o Secret Kubernetes `administrativo-service` a partir do Secret Manager (connection string) e dos org secrets `SERVICE_AUTH_KEY` + `DD_API_KEY` (a mesma key em todos os serviços da malha).

## CD (GCP)

| Workflow | Gatilho | Papel |
|----------|---------|--------|
| `build-push` | push em `main` | Imagem `administrativo-service` no Artifact Registry (BuildKit + `packages: read` para o `building-blocks`) |
| `tf-apply` / `tf-destroy` | `workflow_dispatch` | Instância SQL, database, usuário e secrets SM |
| `deploy` | `workflow_dispatch` | Secret k8s + manifests `k8s/` + rollout + smoke em `https://vcosta-fiap.online/administrativo/health` |

Pré-requisitos do `deploy`:

1. Cluster + Gateway do [`infra-k8s`](https://github.com/fiap-vcosta/infra-k8s) no ar
2. `tf-apply` deste repo já aplicado (instância + database + secrets no SM)
3. Imagem já publicada pelo `build-push`

O `HTTPRoute` expõe só o prefixo público `/administrativo`. Rotas `/system/administrativo/…` ficam no cluster. JWT staff/cliente no ConfigMap valida JWKS em `https://vcosta-fiap.online/autenticacao/.well-known/jwks.json` (iss/aud: `autenticacao-service` / `funcionario`|`cliente`) — o smoke atual é só health.

Requestly: environment **GCP** em [`docs/requestly/`](docs/requestly/) (`baseUrl` = `https://vcosta-fiap.online`).

## Build

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
```
