using EscolaDeCursos.Dominio.Modulos.ModuloProfessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Compartilhado.Orm.Config;

public sealed class ProfessorConfiguration
    : IEntityTypeConfiguration<Professor>
{
    public void Configure(
        EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("TBProfessorPerfil");

        builder.HasKey(p => p.Id)
            .HasName("PK_TBProfessorPerfil");

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Bio)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(p => p.Especialidades)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.DataContratacao)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(p => p.Usuario)
            .WithOne()
            .HasForeignKey<Professor>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_TBProfessorPerfil_TBUsuario"
            );
    }
}
