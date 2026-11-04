# administrativo-service

Serviço do catálogo da oficina: cliente, veículo e serviço. Expõe a API pública do administrativo e uma API interna para os outros serviços consultarem esses dados.

Stack: .NET 8 e PostgreSQL. Org [fiap-vcosta](https://github.com/fiap-vcosta). Pacote compartilhado: [`building-blocks`](https://github.com/fiap-vcosta/building-blocks) **1.0.0** (GitHub Packages).

Requisitos: [docs/01_requisitos.md](docs/01_requisitos.md).

## Layout

| Projeto | Papel |
|---------|--------|
| `src/Domain` | Cliente, Veículo, Serviço + VO `Documento` |
| `src/Application` | Use cases MediatR + gateways |
| `src/Infrastructure` | EF Core (PostgreSQL), migration **sem seed** |
| `src/Api` | Controllers `Staff/` + `SystemApi/` (`/administrativo/…` e `/system/administrativo/…`) |

## Dados de demo

Não há seed. Cadastros via Requestly ([docs/requestly/](docs/requestly/)) ou e2e. Connection string usa `Maximum Pool Size=15`.

## JWT local (teste)

Chave privada **não** vai para o Git.

```bash
./scripts/setup-local-jwt.sh   # cria .local/jwt/*.pem e esboço de .env
./scripts/mint-staff-jwt.sh    # imprime Bearer JWT RS256 (cole em Requestly → token)
```

Em `Development`, a Api carrega `.local/jwt/public.pem` automaticamente se o arquivo existir. No Compose, use `JWT_PUBLIC_KEY_PEM` no `.env` (o setup script preenche).

## Execução local

```bash
# PAT read:packages para restore / docker build
export NUGET_AUTH_TOKEN=…   # ou autentique o feed github no NuGet

dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
./scripts/setup-local-jwt.sh
dotnet run --project src/Api/Api.csproj
# Swagger: http://localhost:8080/swagger
# Health:  http://localhost:8080/administrativo/health
```

Compose (app + Postgres):

```bash
cp .env.example .env   # ou use o .env gerado pelo setup-local-jwt
# preencha NUGET_AUTH_TOKEN
docker compose --profile app up --build
```

## Build

```bash
dotnet restore TechChallenge.sln
dotnet build TechChallenge.sln -c Release
```
