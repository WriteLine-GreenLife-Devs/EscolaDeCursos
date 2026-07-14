using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class AlunoConfiguration
    : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("TBAluno");

        builder.HasKey(aluno => aluno.Id)
            .HasName("PK_TBAluno");

        builder.Property(aluno => aluno.Id)
            .ValueGeneratedNever();

        builder.HasOne(aluno => aluno.Usuario)
            .WithOne()
            .HasForeignKey<Aluno>(aluno => aluno.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TBAluno_TBUsuario");
    }
}