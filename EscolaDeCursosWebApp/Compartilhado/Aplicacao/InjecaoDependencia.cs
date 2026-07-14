using EscolaDeCursosWebApp.Compartilhado.Aplicacao.Logging;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;

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
        services.AddScoped<ServicoCurso>();
        services.AddScoped<ServicoTurma>();
        services.AddScoped<ServicoMatricula>();
        services.AddScoped<ServicoProfessor>();
        services.AddScoped<ServicoAluno>();
        services.AddScoped<ServicoCatalogoAluno>();
        services.AddScoped<ServicoNotaAluno>();
        services.AddScoped<ServicoPresencaAluno>();
    }
}