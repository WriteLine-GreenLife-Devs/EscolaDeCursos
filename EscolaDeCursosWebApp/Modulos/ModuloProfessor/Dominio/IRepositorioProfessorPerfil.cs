using EscolaDeCursosWebApp.Compartilhado.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;

public interface IRepositorioProfessorPerfil
    : IRepositorio<ProfessorPerfil>
{
    void CadastrarProfessor(
        Usuario usuario,
        ProfessorPerfil perfil
    );
}