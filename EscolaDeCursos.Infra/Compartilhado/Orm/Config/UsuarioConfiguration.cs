using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Compartilhado.Orm.Config;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("TBUsuario");

        builder.HasKey(c => c.Id)
            .HasName("PK_TBUsuario");

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.senha)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.telefone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.tipoUsuario)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.ativo)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(c => c.email)
            .IsUnique()
            .HasDatabaseName("UQ_TBUsuario_Email");

        builder.HasIndex(c => c.telefone)
            .IsUnique()
            .HasDatabaseName("UQ_TBUsuario_Telefone");
    }
}
