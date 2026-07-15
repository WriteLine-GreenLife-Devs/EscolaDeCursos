using Microsoft.EntityFrameworkCore;
using EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Infra.Modulos.ModuloAluno;
using EscolaDeCursos.Infra.Modulos.ModuloConteudoCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloProfessor;
using EscolaDeCursos.Infra.Modulos.ModuloCategoria;
using EscolaDeCursos.Infra.Modulos.ModuloMatricula;
using EscolaDeCursos.Infra.Modulos.ModuloProfessor;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using Microsoft.Extensions.DependencyInjection;
using EscolaDeCursos.Infra.Modulos.ModuloUsuario;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.Infra.Modulos.ModuloCurso;
using EscolaDeCursos.Infra.Modulos.ModuloTurma;
using Microsoft.Extensions.Configuration;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Infra;

namespace EscolaDeCursos.Infra.Compartilhado;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        AddOrmRepositories(services, configuration);
    }

    private static void AddOrmRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EscolaDeCursosDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("SqlServerEF");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"A connection string \"SqlServerEF\" não foi encontrada."
                );
            }

            options.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure(3);
            });
        });

        // services.AddScoped<IRepositorioContato, RepositorioContatoEmOrm>();
        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        services.AddScoped<IRepositorioCurso, RepositorioCurso>();
        services.AddScoped<IRepositorioTurma, RepositorioTurma>();
        services.AddScoped<IRepositorioMatricula, RepositorioMatricula>();
        services.AddScoped<IRepositorioProfessor, RepositorioProfessor>();
        services.AddScoped<IRepositorioAluno, RepositorioAluno>();
        services.AddScoped<IRepositorioNotaAluno, RepositorioNotaAluno>();
        services.AddScoped<IRepositorioPresencaAluno, RepositorioPresencaAluno>();
        services.AddScoped<IRepositorioModuloCurso, RepositorioModuloCurso>();
        services.AddScoped<IRepositorioProgressoModuloAluno, RepositorioProgressoModuloAluno>();
    }
}