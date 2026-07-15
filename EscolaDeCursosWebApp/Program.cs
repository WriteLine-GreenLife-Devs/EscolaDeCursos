using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursosWebApp.Compartilhado.Apresentacao;
using EscolaDeCursos.Infra.Compartilhado;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Security.Claims;
using EscolaDeCursos.Aplicacao.Modulos.ModuloUsuario;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configuração do container de injeção de dependência
builder.Services.AddInfraRepositories(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddPresentationConfig(builder.Configuration);

// Autenticação por cookie
builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
        options.Cookie.Name = "EscolaDeCursos.Auth";

        options.Events.OnValidatePrincipal = async contexto =>
        {
            string? identificadorUsuario = contexto.Principal?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            bool usuarioIdValido = Guid.TryParse(
                identificadorUsuario,
                out Guid usuarioId);

            ServicoUsuario servicoUsuario = contexto.HttpContext
                .RequestServices
                .GetRequiredService<ServicoUsuario>();

            if (usuarioIdValido &&
                servicoUsuario.VerificarUsuarioAtivo(usuarioId))
            {
                return;
            }

            contexto.RejectPrincipal();

            await contexto.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireADMRole", policy => policy.RequireRole("ADM"));
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<EscolaDeCursosDbContext>(
        name: "database_check",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]
    );

var app = builder.Build();

// Middlewares de arquivos estáticos e roteamento
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapHealthChecks("/health");

// Execução do Servidor
app.Run();
