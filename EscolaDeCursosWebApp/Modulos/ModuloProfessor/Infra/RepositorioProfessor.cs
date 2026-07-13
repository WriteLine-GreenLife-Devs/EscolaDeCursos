using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Infra;

public sealed class RepositorioProfessor(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<Professor>(dbContext),
      IRepositorioProfessor
{
    private readonly EscolaDeCursosDbContext contexto = dbContext;

    public void CadastrarProfessor(
        Usuario usuario,
        Professor professor)
    {
        professor.Usuario = usuario;

        contexto.Usuarios.Add(usuario);
        contexto.Professores.Add(professor);
        contexto.SaveChanges();
    }
}
