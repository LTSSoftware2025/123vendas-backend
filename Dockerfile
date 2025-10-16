FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["123Vendas.Api/123Vendas.Api.csproj", "123Vendas.Api/"]
COPY ["123Vendas.Application/123Vendas.Application.csproj", "123Vendas.Application/"]
COPY ["123Vendas.Domain/123Vendas.Domain.csproj", "123Vendas.Domain/"]
COPY ["123Vendas.Infra/123Vendas.Infra.csproj", "123Vendas.Infra/"]

RUN dotnet restore "123Vendas.Api/123Vendas.Api.csproj"

COPY . .

WORKDIR "/src/123Vendas.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

EXPOSE 5103

ENV ASPNETCORE_URLS=http://+:5103
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["bash", "/wait-for-it.sh", "sqlserver", "dotnet 123Vendas.Api.dll"]

