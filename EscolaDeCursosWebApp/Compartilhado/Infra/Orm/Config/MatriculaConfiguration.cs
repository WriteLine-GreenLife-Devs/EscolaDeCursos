using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class MatriculaConfiguration : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("TBMatricula");

        builder.HasKey(m => m.Id)
            .HasName("PK_TBMatricula");

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.AlunoId)
            .IsRequired();

        builder.Property(m => m.TurmaId)
            .IsRequired();

        builder.Property(m => m.DataMatricula)
            .IsRequired();

        builder.Property(m => m.Situacao)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(m => m.Nota1)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(m => m.Nota2)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(m => m.Nota3)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(m => m.Recuperacao)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(m => m.NotaFinal)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.HasIndex(m => m.TurmaId)
            .HasDatabaseName("IX_TBMatricula_TurmaId");

        builder.HasIndex(m => m.AlunoId)
            .HasDatabaseName("IX_TBMatricula_AlunoId");
    }
}
