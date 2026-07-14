using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Infra;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Infra;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursosWebApp.Compartilhado.Infra;

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