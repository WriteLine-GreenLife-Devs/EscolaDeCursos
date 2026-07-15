using EscolaDeCursos.Dominio.Modulos.ModuloProfessor;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloProfessor;

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
