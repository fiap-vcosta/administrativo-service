# Requisitos — administrativo-service

Sessão de Outubro de 2026. Este serviço é o dono de cliente, veículo e do catálogo de serviço (mão de obra). Persistência em PostgreSQL. Não publica eventos. Não é dono de ordem de serviço, estoque, orçamento nem de emissão de JWT.

Paths e corpos internos: [HTTP](https://github.com/fiap-vcosta/e2e/blob/main/docs/contratos-http.md).

## 1. Rotas públicas

Exigem JWT staff. O prefixo é `/administrativo`.

| Método | Path |
|--------|------|
| `POST` | `/administrativo/clientes` |
| `GET` | `/administrativo/clientes` |
| `GET` | `/administrativo/clientes/{id}` |
| `GET` | `/administrativo/clientes/{id}/veiculos` |
| `PUT` | `/administrativo/clientes/{id}` |
| `DELETE` | `/administrativo/clientes/{id}` |
| `POST` | `/administrativo/veiculos` |
| `GET` | `/administrativo/veiculos` |
| `GET` | `/administrativo/veiculos/{id}` |
| `PUT` | `/administrativo/veiculos/{id}` |
| `DELETE` | `/administrativo/veiculos/{id}` |
| `POST` | `/administrativo/servicos` |
| `GET` | `/administrativo/servicos` |
| `GET` | `/administrativo/servicos/{id}` |
| `PUT` | `/administrativo/servicos/{id}` |
| `DELETE` | `/administrativo/servicos/{id}` |

## 2. Rotas internas

Existem só na rede do cluster. Exigem o header `X-Service-Key`. Chave ausente ou diferente responde 401. Ausente no GET por id responde 404.

* **RF01:** `GET /system/administrativo/clientes/por-documento/{documento}` atende o `autenticacao-service`. `documento` é CPF ou CNPJ, normalizado antes da busca. Inválido responde 400 com `{ "errors": ["..."] }`. Existente responde 200 sem corpo. Ausente responde 404.
* **RF02:** `GET /system/administrativo/clientes/{id}` responde `id`, `nome`, `email`, `tipoDocumento` (`Cpf` ou `Cnpj`) e `documento`.
* **RF03:** `GET /system/administrativo/veiculos/{id}` responde `id`, `idCliente`, `placa`, `marca` e `modelo`.
* **RF04:** `GET /system/administrativo/servicos/{id}` responde `id`, `codigo`, `nome` e `precoPadrao`.
