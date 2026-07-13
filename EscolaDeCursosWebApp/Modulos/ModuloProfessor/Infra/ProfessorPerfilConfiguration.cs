using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Infra;

public sealed class ProfessorPerfilConfiguration
    : IEntityTypeConfiguration<ProfessorPerfil>
{
    public void Configure(
        EntityTypeBuilder<ProfessorPerfil> builder)
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
            .HasForeignKey<ProfessorPerfil>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_TBProfessorPerfil_TBUsuario"
            );
    }
}