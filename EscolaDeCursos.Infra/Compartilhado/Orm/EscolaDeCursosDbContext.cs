using Microsoft.EntityFrameworkCore;
using EntidadeModuloCurso = EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso.ModuloCurso;
using ProgressoModuloAluno = EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso.ProgressoModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloProfessor;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;

namespace EscolaDeCursos.Infra.Compartilhado.Orm;

public sealed class EscolaDeCursosDbContext(DbContextOptions<EscolaDeCursosDbContext> options) : DbContext(options)
{
    // public DbSet<Contato> Contatos => Set<Contato>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Turma> Turmas => Set<Turma>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Professor> Professores => Set<Professor>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<NotaAluno> NotasAluno => Set<NotaAluno>();
    public DbSet<PresencaAluno> PresencasAluno => Set<PresencaAluno>();
    public DbSet<EntidadeModuloCurso> ModulosCurso => Set<EntidadeModuloCurso>();
    public DbSet<ProgressoModuloAluno> ProgressosModuloAluno => Set<ProgressoModuloAluno>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaDeCursosDbContext).Assembly);
    }
}