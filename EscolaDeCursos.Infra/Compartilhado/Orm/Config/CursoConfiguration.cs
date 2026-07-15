using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Compartilhado.Orm.Config;

public sealed class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("TBCurso");

        builder.HasKey(c => c.Id)
            .HasName("PK_TBCurso");

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.descricao)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.cargaHoraria)
            .IsRequired();

        builder.Property(c => c.nivelDificuldade)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.valor)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.categoriaId)
            .IsRequired();

        builder.HasIndex(c => c.nome)
            .IsUnique()
            .HasDatabaseName("UQ_TBCurso_Nome");
    }
}
