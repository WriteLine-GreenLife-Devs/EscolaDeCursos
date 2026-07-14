using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class ProgressoModuloAlunoConfiguration
    : IEntityTypeConfiguration<ProgressoModuloAluno>
{
    public void Configure(
        EntityTypeBuilder<ProgressoModuloAluno> builder)
    {
        builder.ToTable("TBProgressoModuloAluno", tabela =>
        {
            tabela.HasCheckConstraint(
                "CK_TBProgressoModuloAluno_Conclusao",
                "([Concluido] = 1 AND [DataConclusao] IS NOT NULL) OR " +
                "([Concluido] = 0 AND [DataConclusao] IS NULL)"
            );
        });

        builder.HasKey(progresso => progresso.Id)
            .HasName("PK_TBProgressoModuloAluno");

        builder.Property(progresso => progresso.Id)
            .ValueGeneratedNever();

        builder.Property(progresso => progresso.MatriculaId)
            .IsRequired();

        builder.Property(progresso => progresso.ModuloCursoId)
            .IsRequired();

        builder.Property(progresso => progresso.Concluido)
            .IsRequired();

        builder.Property(progresso => progresso.DataConclusao);

        builder.HasOne<Matricula>()
            .WithMany()
            .HasForeignKey(progresso => progresso.MatriculaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_TBProgressoModuloAluno_TBMatricula"
            );

        builder.HasOne<ModuloCurso>()
            .WithMany()
            .HasForeignKey(progresso => progresso.ModuloCursoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_TBProgressoModuloAluno_TBModuloCurso"
            );

        builder.HasIndex(progresso => progresso.ModuloCursoId)
            .HasDatabaseName(
                "IX_TBProgressoModuloAluno_ModuloCursoId"
            );

        builder.HasIndex(progresso => new
            {
                progresso.MatriculaId,
                progresso.ModuloCursoId
            })
            .IsUnique()
            .HasDatabaseName(
                "UQ_TBProgressoModuloAluno_MatriculaId_ModuloCursoId"
            );
    }
}