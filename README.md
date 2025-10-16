# 123vendas-backend
API de Vendas desenvolvida em .NET 9 para avaliação técnica da Ambev Tech utilizando o Clean Architecture e o DDD. O projeto traz uma série de testes unitários, logs estruturados com Serilog e observabilidade via OpenTelemetry.

## Pacotes (NuGet)
Foi adicionado na raiz do projeto um arquivo NuGet.config afim de garantir que nenhum outro pacote de feed privado seja utilizado.
Para efetuar a restauração, basta executar o seguinte comando: 
```
dotnet restore --configfile NuGet.config
```

## Estrutura do Projeto
- `123Vendas/Api` — Camada de apresentação
- `123Vendas/Application` — Casos de uso, DTOs, validações
- `123Vendas/Domain` — Entidades, regras de negócios
- `123Vendas/Infra` — EF Core, repositórios, logs
- `123Vendas/XUnitTest/Domain` — Testes unitários do domínio
- `123Vendas/XUnitTest/Api` — Testes unitários da Api

## Domain Events
Foi adicionado ao projeto todo o mecanismo responsável pela publicação de eventos na camada Domain, seguido os princípios de DDD.
Com essa implementação, será possível gerar eventos rastreáveis em mudanças que sejam relevantes no domínio, como por exemplo criação e cancelamento de vendas.

## Testes unitários
Os testes de domínio foram ampliados afim de atender 100% da cobertura lógica do domínio.

- `VendaTests` — Valida a criação, adição e remoção de itens, e invariantes (como impedir alterações após cancelamento)
- `VendaEventosTests` — Garante que eventos adequados sejam publicados em cada uma das operações relevantes
- `ItemVendaTests` — Valida questões sobre quantidade, valor unitário e cálculo de descontos de acordo com as regras de negócio
- `RegrasDescontoTests` — Garante que as regras referente aos percentuais de desconto estejam sendo respeitadas de acordo com cada quantidade especificada no negócio

## Infra (Entity Framework)
A camada de infraestrutura foi implementada utilizando Entity Framework Core com SQL Server LocalDB.

### Configuração
- `Banco de Dados` — VendasDb
- `Provider` — EntityFrameworkCore.SqlServer
- `Contexto` — ApplicationDbContext
- `Migrações` — As migrações estão localizadas em: 123Vendas.Infra/Migrations

### Logging
Foi implementado com Serilog, gravando logs no console (JSON format).
As configurações do logging está em appsettings.json

### Como testar?
Para executar o projeto, siga o passo a passo abaixo:

Passo 1: Executar o comando para criar o banco de dados
```
dotnet ef database update
```

Passo 2: Iniciar a API
```
dotnet run --project 123Vendas.Api
```

Passo 3: Verificar o endpoint de status da aplicação
```
http://localhost:5103/health
```

Passo 4: Confira no SQL Server que as tabelas venda e itemvenda foram criadas corretamente