using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("TBCategoria");

        builder.HasKey(c => c.Id)
            .HasName("PK_TBCategoria");

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.descricao)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(c => c.nome)
            .IsUnique()
            .HasDatabaseName("UQ_TBCategoria_Nome");
    }
}
