using _123Vendas.Application.DTOs;
using _123Vendas.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.Api.Endpoints;

public static class VendasEndpoints
{
    public static void MapVendasEndpoints(this IEndpointRouteBuilder app)
    {
        var vendas = app.MapGroup("/vendas").WithTags("Vendas");

        vendas.MapPost("/", async (
            IServiceVendas serviceVendas,
            [FromBody] CriarVendaDtoRequest request,
            CancellationToken cancellationToken) =>
        {
            var id = await serviceVendas.CriarAsync(request, cancellationToken);
            return Results.Created($"/vendas/{id}", new { id });
        });

        vendas.MapGet("/{id:guid}", async (
            IServiceVendas serviceVendas,
            Guid id,
            CancellationToken cancellationToken) =>
        {
            var venda = await serviceVendas.ObterPorIdAsync(id, cancellationToken);
            return venda is null ? Results.NotFound() : Results.Ok(venda);
        });

        vendas.MapGet("/por-numero/{numero}", async (
            IServiceVendas serviceVendas,
            string numero,
            CancellationToken cancellationToken) =>
        {
            var venda = await serviceVendas.ObterPorNumeroVendaAsync(numero, cancellationToken);
            return venda is null ? Results.NotFound() : Results.Ok(venda);
        });

        vendas.MapGet("/", async (
            IServiceVendas serviceVendas,
            int pagina = 1,
            int tamanho = 20,
            CancellationToken cancellationToken = default) =>
        {
            var lista = await serviceVendas.ListarAsync(pagina, tamanho, cancellationToken);
            return Results.Ok(lista);
        });

        vendas.MapPut("/", async (
            IServiceVendas serviceVendas,
            [FromBody] AtualizarVendaDtoRequest request,
            CancellationToken cancellationToken) =>
        {
            await serviceVendas.AtualizarAsync(request, cancellationToken);
            return Results.NoContent();
        });

        vendas.MapPost("/{id:guid}/itens", async (
            IServiceVendas serviceVendas,
            Guid id,
            [FromBody] AdicionarItemVendaDtoRequest item,
            CancellationToken cancellationToken) =>
        {
            await serviceVendas.AdicionarItemAsync(id, item, cancellationToken);
            return Results.NoContent();
        });

        vendas.MapDelete("/{id:guid}/itens/{itemId:guid}", async (
            IServiceVendas serviceVendas,
            Guid id,
            Guid itemId,
            CancellationToken cancellationToken) =>
        {
            await serviceVendas.RemoverItemAsync(id, itemId, cancellationToken);
            return Results.NoContent();
        });

        vendas.MapPost("/{id:guid}/cancelar", async (
            IServiceVendas serviceVendas,
            Guid id,
            CancellationToken cancellationToken) =>
        {
            await serviceVendas.CancelarAsync(id, cancellationToken);
            return Results.NoContent();
        });
    }
}
