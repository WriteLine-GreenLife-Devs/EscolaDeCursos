using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursosWebApp.Compartilhado.Infra.Orm.Config;

public sealed class TurmaConfiguration : IEntityTypeConfiguration<Turma>
{
    public void Configure(EntityTypeBuilder<Turma> builder)
    {
        builder.ToTable("TBTurma");

        builder.HasKey(t => t.Id)
            .HasName("PK_TBTurma");

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.dataInicio)
            .IsRequired();

        builder.Property(t => t.dataFim)
            .IsRequired();

        builder.Property(t => t.vagasMaximas)
            .IsRequired();

        builder.Property(t => t.HorarioTurno)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.cursoId)
            .IsRequired();

        builder.Property(t => t.instrutorId)
            .IsRequired();

        builder.HasIndex(t => t.nome)
            .HasDatabaseName("IX_TBTurma_Nome");
    }
}
