# 123vendas-backend
API de Vendas desenvolvida em .NET 9 para avaliação técnica da Ambev Tech utilizando o Clean Architecture e o DDD. O projeto traz uma série de testes unitários, logs estruturados com Serilog e observabilidade via OpenTelemetry.

## Pacotes (NuGet)
Foi adicionado na raiz do projeto um arquivo NuGet.config afim de garantir que nenhum outro pacote de feed privado seja utilizado.
Para efetuar a restauração, basta executar o seguinte comando: dotnet restore --configfile NuGet.config
