using EscolaDeCursosWebApp.Compartilhado.Aplicacao.Logging;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Infra;

namespace EscolaDeCursosWebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder logging
    )
    {
        services.AddSerilogLogger(configuration, logging);

        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<ServicoUsuario>();
        services.AddScoped<ServicoCategoria>();
    }
}
