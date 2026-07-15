using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Compartilhado.Orm.Config;

public sealed class PresencaAlunoConfiguration
    : IEntityTypeConfiguration<PresencaAluno>
{
    public void Configure(EntityTypeBuilder<PresencaAluno> builder)
    {
        builder.ToTable("TBPresencaAluno");

        builder.HasKey(presenca => presenca.Id)
            .HasName("PK_TBPresencaAluno");

        builder.Property(presenca => presenca.Id)
            .ValueGeneratedNever();

        builder.Property(presenca => presenca.AlunoId)
            .IsRequired();

        builder.Property(presenca => presenca.MatriculaId)
            .IsRequired();

        builder.Property(presenca => presenca.DataAula)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(presenca => presenca.Presente)
            .IsRequired();

        builder.HasOne(presenca => presenca.Aluno)
            .WithMany(aluno => aluno.Presencas)
            .HasForeignKey(presenca => presenca.AlunoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TBPresencaAluno_TBAluno");

        builder.HasOne<Matricula>()
            .WithMany()
            .HasForeignKey(presenca => presenca.MatriculaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TBPresencaAluno_TBMatricula");

        builder.HasIndex(presenca => presenca.AlunoId)
            .HasDatabaseName("IX_TBPresencaAluno_AlunoId");

        builder.HasIndex(presenca => new
            {
                presenca.MatriculaId,
                presenca.DataAula
            })
            .IsUnique()
            .HasDatabaseName("UQ_TBPresencaAluno_MatriculaId_DataAula");
    }
}