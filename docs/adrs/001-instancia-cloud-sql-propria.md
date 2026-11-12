# ADR 001: Instância Cloud SQL própria do serviço

**Data:** 12 de novembro de 2026  
**Status:** Aceito  
**Autores:** Victor Costa

## 1. Contexto e Problema

Na Fase 04 a instância PostgreSQL era compartilhada (`infra-db`, `tech-challenge-pg`), com um database/usuário por serviço SQL e isolamento lógico (`REVOKE CONNECT` / role NOLOGIN). O enunciado pede banco próprio por microsserviço; isolamento lógico não basta para a narrativa da entrega.

O problema a ser resolvido é: **Onde vive a instância Cloud SQL do `administrativo-service` e com qual custo/hardening mínimo?**

## 2. Decisão

- **Instância** `administrativo-service-pg` no Terraform deste repo (`db-f1-micro`, PostgreSQL 16, IP privado via VPC do `infra-bootstrap`, sem HA, backup desligado, `deletion_protection = false`).
- **Database** `administrativo` e usuário `administrativo` no mesmo stack; connection string no Secret Manager; Auth Proxy no GKE.
- **Pool Npgsql** `Maximum Pool Size=4` (teto ~25 conexões do `f1-micro`).
- **Harden:** só grants de schema/`CONNECT` para o usuário da app (Data API). Sem role NOLOGIN, sem `REVOKE` em `template1`, sem isolamento cross-database.
- **Rede:** `terraform_remote_state` do bootstrap — este repo não cria VPC.
- **Ciclo:** sobe/desce com o `tf-apply` / `tf-destroy` deste serviço (não depende mais de `infra-db`).

## 3. Justificativa

* Alinha “infraestrutura e banco” no mesmo repositório do serviço.
* `db-f1-micro` basta com um serviço por instância e pool baixo; o `db-g1-small` era necessário só no modelo compartilhado.
* Grants mínimos evitam falha do EF migrate sem o peso do harden cross-DB.

## 4. Alternativas Consideradas

* **Manter instância compartilhada no `infra-db`:** menor custo horário com um SQL; rejeitado para alinhar ao PDF e à autonomia por serviço.
* **N stacks no `infra-db`:** centraliza SQL; rejeitado — o dono natural do banco do serviço é o repo do serviço.
* **Sem Data API / grants:** risco de migrate falhar no `public` do PostgreSQL 16.

## 5. Consequências

### Positivas
* Demo e README mostram banco próprio por serviço.
* Destroy do serviço apaga a instância; sem órfão no `infra-db`.

### Negativas / Riscos
* **~3× compute SQL** na janela se estoque e ordem de serviço espelharem o padrão.
  * *Mitigação:* `demo-down` obrigatório; tier `f1-micro`.
* **Nome da instância reservado** alguns dias após destroy.
  * *Mitigação:* aguardar ou alterar `db_instance_name`.
