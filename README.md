# FSIT.PayLink

**FSIT.PayLink** é uma API REST minimalista em .NET 9 para geração e acompanhamento de cobranças Pix via AbacatePay (ou provider fake), usando Onion Architecture (Domain, Application, Infrastructure) e EF Core para persistência.

## Funcionalidades

* Criação de cobranças Pix (`POST /charges`)
* Consulta de status de cobrança (`GET /charges/{id}`)
* Webhook para notificações de pagamento (`POST /webhook/abacatepay`)
* Suporte a multi-tenant via header `x-tenant-id`

## Estrutura de projetos

* **FSIT.PayLink.Api**: ponto de entrada, configura serviços e endpoints.
* **FSIT.PayLink.Application**: regras de negócio, comandos, consultas e mensageria.
* **FSIT.PayLink.Domain**: entidades, value objects e repositórios.
* **FSIT.PayLink.Infrastructure**: implementações de repositórios, provider de pagamento (AbacatePay/Fake), EF Core e eventos.

## Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL](https://www.postgresql.org/download/) local ou via Docker
* Opcional: [Docker & Docker Compose](https://docs.docker.com/compose/)
* Opcional: [ngrok](https://ngrok.com/) para expor localmente o webhook

## Configuração

Copie o arquivo de exemplo e ajuste as credenciais:

```bash
cp src/FSIT.PayLink.Api/appsettings.json.sample src/FSIT.PayLink.Api/appsettings.json
```

Em `appsettings.json`: configure a string de conexão, chave da API Abacate e `WebhookSecret`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=paylink;Username=postgres;Password=postgres"
  },
  "Abacate": {
    "BaseUrl": "https://api.abacatepay.com",
    "ApiKey": "abc_dev_HFcqdUpctFPqBFPYt6mcYJWK",
    "DefaultCurrency": "BRL",
    "WebhookSecret": "69CA52D0-ECE8-48B1-8748-DE8B7B86BE96"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

> **Importante:** `appsettings.json` NÃO deve ser versionado; mantenha apenas o `appsettings.json.sample` no repositório.

## Executando localmente

### Banco de dados

1. Crie o banco PostgreSQL (local) ou use Docker:

```bash
docker run -d --name paylink-db -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres:16
```

2. Aplique as migrações:

```bash
cd src/FSIT.PayLink.Api
dotnet ef database update --connection "Host=localhost;Port=5432;Database=paylink;Username=postgres;Password=postgres"
```

### API

```bash
cd src/FSIT.PayLink.Api
dotnet run
```

A API ficará disponível em `https://localhost:7082`.

## Testando o Webhook com ngrok

Para receber chamadas do AbacatePay no seu ambiente local, exponha a porta HTTPS com o ngrok:

```bash
ngrok http https://localhost:7082
```

Copie a URL gerada (por exemplo `https://94e572f020cf.ngrok-free.app`) e configure no dashboard do AbacatePay em **Integração > Webhook** apontando para:

```
https://<seu-ngrok-url>/webhook/abacatepay?webhookSecret=<WebhookSecret>
```

## Exemplos de uso

### Criar cobrança

```bash
curl -X POST https://localhost:7082/charges \
  -H "Content-Type: application/json" \
  -H "x-tenant-id: 00000000-0000-0000-0000-000000000001" \
  -d '{
    "amount": 15.00,
    "currency": "BRL",
    "tenantId": "00000000-0000-0000-0000-000000000001",
    "cpf": "11144477735"
  }'
```

### Consultar status

```bash
curl https://localhost:7082/charges/<paymentId> \
  -H "x-tenant-id: 00000000-0000-0000-0000-000000000001"
```

### Simular webhook

```bash
curl -X POST https://<seu-ngrok-url>/webhook/abacatepay?webhookSecret=69CA52D0-ECE8-48B1-8748-DE8B7B86BE96 \
  -H "Content-Type: application/json" \
  -d '{ ... payload billing.paid ... }'
```

## Próximos passos

* Implementar validação de CPF e janela de duplicidade
* Suportar múltiplos gateways
* Adicionar testes automatizados (unitários e de integração)

---

*Feito com ♥ por Vinicius*
