using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Compartilhado.Apresentacao;
using EscolaDeCursosWebApp.Compartilhado.Infra;
using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configuração do container de injeção de dependência
builder.Services.AddInfraRepositories(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration, builder.Logging);

builder.Services.AddPresentationConfig(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<EscolaDeCursosDbContext>(
        name: "database_check",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]
    );

var app = builder.Build();

// Middlewares de roteamento
app.UseRouting();
app.MapDefaultControllerRoute();

app.MapHealthChecks("/health");

// Execução do Servidor
app.Run();
