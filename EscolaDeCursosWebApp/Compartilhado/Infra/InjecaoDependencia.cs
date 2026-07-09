using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
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
    }
}
