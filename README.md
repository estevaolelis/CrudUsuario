# Argentum API

API RESTful para gestão financeira, desenvolvida com **.NET 10** e **Entity Framework Core**. O sistema gerencia clientes, contas bancárias e transações, incluindo funcionalidades de exportação de relatórios em Excel/CSV.

## 🚀 Funcionalidades

* **Gestão de Clientes:** CRUD completo de clientes.
* **Contas Bancárias:** Associação de contas a clientes com controle de saldo.
* **Relatórios:** Exportação de dados para Excel (.xlsx) e CSV utilizando *ClosedXML*.
* **Arquitetura:** Separação em Camadas (Controllers, Services, Models, Data).
* **Documentação:** Interface interativa com Swagger UI.

## 🛠 Tecnologias Utilizadas

* **Linguagem:** C# (.NET 10)
* **ORM:** Entity Framework Core
* **Banco de Dados:** PostgreSQL (Hospedado no Neon.tech)
* **Ferramentas:**
    * *Newtonsoft.Json* (Serialização)
    * *ClosedXML* (Manipulação de Excel)
    * *Swagger/OpenAPI* (Documentação)

## ⚙️ Configuração

1. Clone o repositório.
2. Crie um arquivo `appsettings.json` na raiz do projeto seguindo o modelo abaixo:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=SEU_HOST_NEON;Database=neondb;Username=SEU_USER;Password=SUA_SENHA;SSL Mode=Require;Trust Server Certificate=true"
  }
}
