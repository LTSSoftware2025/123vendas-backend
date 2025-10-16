using _123Vendas.Api.Events;
using _123Vendas.Domain.Events.Dispatcher;
using _123Vendas.Domain.Interfaces;
using _123Vendas.Infra.Context;
using _123Vendas.Infra.Context.Mappers;
using _123Vendas.Infra.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console(new CompactJsonFormatter());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddAutoMapper(typeof(VendaMapper).Assembly);

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

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
     
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        // Teste rápido de conexão com o banco
        var connectionString = app.Configuration.GetConnectionString("SqlServer");
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        logger.LogInformation("Banco de dados conectado com sucesso ({Database})", dbContext.Database.GetDbConnection().Database);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao conectar ou migrar o banco de dados.");
    }
}

// Log limpo de inicialização
var environment = app.Environment.EnvironmentName;
var urls = app.Urls.Any() ? string.Join(" | ", app.Urls) : "URLs não configuradas";

app.Logger.LogInformation("""
--------------------------------------------------------
API 123Vendas iniciada com sucesso!
Endpoints: {Urls}
Ambiente: {Environment}
--------------------------------------------------------
""", urls, environment);


app.Run();
