using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class NotaAlunoConfiguration
    : IEntityTypeConfiguration<NotaAluno>
{
    public void Configure(EntityTypeBuilder<NotaAluno> builder)
    {
        builder.ToTable("TBNotaAluno");

        builder.HasKey(nota => nota.Id)
            .HasName("PK_TBNotaAluno");

        builder.Property(nota => nota.Id)
            .ValueGeneratedNever();

        builder.Property(nota => nota.AlunoId)
            .IsRequired();

        builder.Property(nota => nota.MatriculaId)
            .IsRequired();

        builder.Property(nota => nota.TipoNota)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(nota => nota.Descricao)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(nota => nota.Valor)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(nota => nota.DataLancamento)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(nota => nota.Aluno)
            .WithMany(aluno => aluno.Notas)
            .HasForeignKey(nota => nota.AlunoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TBNotaAluno_TBAluno");

        builder.HasOne<Matricula>()
            .WithMany()
            .HasForeignKey(nota => nota.MatriculaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TBNotaAluno_TBMatricula");

        builder.HasIndex(nota => nota.AlunoId)
            .HasDatabaseName("IX_TBNotaAluno_AlunoId");

        builder.HasIndex(nota => new
            {
                nota.MatriculaId,
                nota.TipoNota
            })
            .IsUnique()
            .HasDatabaseName("UQ_TBNotaAluno_MatriculaId_TipoNota");
    }
}