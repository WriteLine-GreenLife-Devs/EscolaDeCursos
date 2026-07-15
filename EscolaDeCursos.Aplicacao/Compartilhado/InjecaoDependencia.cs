using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;
using EscolaDeCursos.Aplicacao.Modulos.ModuloConteudoCurso;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloProfessor;
using EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;
using EscolaDeCursos.Aplicacao.Modulos.ModuloUsuario;
using Microsoft.Extensions.DependencyInjection;

namespace EscolaDeCursos.Aplicacao.Compartilhado;

public static class InjecaoDependencia
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
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
        services.AddScoped<ServicoModuloCurso>();
        services.AddScoped<ServicoProgressoModuloAluno>();

        return services;
    }
}