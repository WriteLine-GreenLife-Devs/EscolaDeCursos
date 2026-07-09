using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm;

public sealed class EscolaDeCursosDbContext(DbContextOptions<EscolaDeCursosDbContext> options) : DbContext(options)
{
    // public DbSet<Contato> Contatos => Set<Contato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaDeCursosDbContext).Assembly);
    }
}
