using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EntidadeModuloCurso = EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio.ModuloCurso;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class ModuloCursoConfiguration
    : IEntityTypeConfiguration<EntidadeModuloCurso>
{
    public void Configure(
        EntityTypeBuilder<EntidadeModuloCurso> builder)
    {
        builder.ToTable("TBModuloCurso", tabela =>
        {
            tabela.HasCheckConstraint(
                "CK_TBModuloCurso_DuracaoMinutos",
                "[DuracaoMinutos] > 0"
            );

            tabela.HasCheckConstraint(
                "CK_TBModuloCurso_Ordem",
                "[Ordem] > 0"
            );
        });

        builder.HasKey(modulo => modulo.Id)
            .HasName("PK_TBModuloCurso");

        builder.Property(modulo => modulo.Id)
            .ValueGeneratedNever();

        builder.Property(modulo => modulo.CursoId)
            .IsRequired();

        builder.Property(modulo => modulo.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(modulo => modulo.Descricao)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(modulo => modulo.DuracaoMinutos)
            .IsRequired();

        builder.Property(modulo => modulo.Ordem)
            .IsRequired();

        builder.Property(modulo => modulo.Ativo)
            .IsRequired();

        builder.HasOne<Curso>()
            .WithMany()
            .HasForeignKey(modulo => modulo.CursoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TBModuloCurso_TBCurso");

        builder.HasIndex(modulo => modulo.CursoId)
            .HasDatabaseName("IX_TBModuloCurso_CursoId");

        builder.HasIndex(modulo => new
            {
                modulo.CursoId,
                modulo.Ordem
            })
            .IsUnique()
            .HasFilter("[Ativo] = 1")
            .HasDatabaseName("UQ_TBModuloCurso_CursoId_Ordem_Ativo");
    }
}