using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm;

public sealed class EscolaDeCursosDbContext(DbContextOptions<EscolaDeCursosDbContext> options) : DbContext(options)
{
    // public DbSet<Contato> Contatos => Set<Contato>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaDeCursosDbContext).Assembly);
    }
}
