using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloAluno;

public sealed class RepositorioAluno(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<Aluno>(dbContext),
      IRepositorioAluno
{
    private readonly EscolaDeCursosDbContext contexto = dbContext;

    public void CadastrarAluno(
        Usuario usuario,
        Aluno aluno)
    {
        aluno.Usuario = usuario;

        contexto.Usuarios.Add(usuario);
        contexto.Alunos.Add(aluno);
        contexto.SaveChanges();
    }
}