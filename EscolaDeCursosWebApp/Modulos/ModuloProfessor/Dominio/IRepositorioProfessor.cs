using EscolaDeCursosWebApp.Compartilhado.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;

public interface IRepositorioProfessor
    : IRepositorio<Professor>
{
    void CadastrarProfessor(
        Usuario usuario,
        Professor professor
    );
}
