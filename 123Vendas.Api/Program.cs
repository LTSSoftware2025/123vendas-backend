using _123Vendas.Api.Endpoints;
using _123Vendas.Api.Events;
using _123Vendas.Application.DTOs;
using _123Vendas.Application.Interface;
using _123Vendas.Application.Services;
using _123Vendas.Domain.Events.Dispatcher;
using _123Vendas.Domain.Interfaces;
using _123Vendas.Infra.Context;
using _123Vendas.Infra.Context.Mappers;
using _123Vendas.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Formatting.Compact;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console(new CompactJsonFormatter());
});

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__SqlServer")
    ?? builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(VendaMapper).Assembly);
builder.Services.AddAutoMapper(typeof(VendaDtoProfile).Assembly);

builder.Services.AddScoped<IServiceVendas, ServiceVendas>();

builder.Services.AddScoped<IRepositoryVenda, RepositoryVenda>();

builder.Services.AddScoped<IDomainEventDispatcher, SerilogDomainEventDispatcher>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resourceBuilder => resourceBuilder.AddService("123Vendas.Api"))
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(meterProviderBuilder => meterProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddConsoleExporter());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "123Vendas API v1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.MapVendasEndpoints();

app.Run();
