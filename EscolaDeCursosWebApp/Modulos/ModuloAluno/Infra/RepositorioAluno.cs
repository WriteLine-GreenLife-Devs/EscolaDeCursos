using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Infra;

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